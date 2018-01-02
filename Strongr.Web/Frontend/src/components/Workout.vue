<template>

<div class="container">
<div v-if="errorMessage" class="alert alert-danger alert-dismissible fade show" role="alert">
  <strong>Error</strong> {{ errorMessage }}
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>

<div v-if="loadingWorkout" class="text-center">
    <img src="/static/images/spinner.gif" height="256" width="256"/>
    <p class="text-muted">{{ loadingWorkoutText }}</p>
</div>

<div v-else>
<div>
    <b-dropdown class="float-right" text="...">
        <b-dropdown-item>
            <span class="fa fa-edit"></span> Edit workout
        </b-dropdown-item>
        <b-dropdown-item v-if="activities.length" @click="$refs.confirmCopy.show()">
            <span class="fa fa-copy"></span> Copy workout
        </b-dropdown-item>
        <b-dropdown-divider />
        <b-dropdown-item-button v-on:click="deleteWorkout()">
            <span class="fa fa-trash"></span> Delete workout
        </b-dropdown-item-button>
    </b-dropdown>
</div>

<b-modal ref="confirmCopy" title="Confirm new workout" @ok="copyWorkout">
    <p class="my-4">Click OK to start a new workout with the activities of this workout.</p>
</b-modal>

<div class="row">
<h2>Workout details</h2>
<small class="startDateTimeheader"></small>

<form v-on:submit.prevent="onSubmit" class="form-inline collapse well" id="workoutDetailsEditor">
    <div class="form-group">
        <input type="hidden" id="StartDateTime-ISO" :value="startDateTimeIsoFormatted" />
        <label asp-for="StartDateTime" class="control-label"></label>
        <div class='input-group date' id='startedDateTimeInput'>
            <span class="input-group-addon">
                <span class="glyphicon glyphicon-calendar"></span>
            </span>
            <input asp-for="StartDateTime" class="form-control" />
            <span class="input-group-btn">
                <button type="submit" class="btn btn-primary" v-on:click="saveWorkout()">Update</button>
            </span>
        </div>

        <span asp-validation-for="StartDateTime" class="text-danger"></span>
    </div>
</form>
</div>

<div class="card">
  <div v-if="isNewActivity" class="card-header">
    Add activity
  </div>
  <div v-else class="card-header">
    Update activity
  </div>
  <div class="card-body">
   <div class="col-12">
        <div v-if="processingActivity" class="text-center">
            <img src="/static/images/spinner.gif" height="256" width="256"/>
            <p class="text-muted">Loading activity...</p>
        </div>
         <form v-else  class="form" v-on:submit.prevent="finishActivity()">
            <div class="form-group">
                <label for="exercise">Exercise</label>
                <v-select v-model="selectedExercise"
                    :disabled="!$store.state.exercises"
                    label="name"
                    :options="$store.state.exercises"
                    placeholder="Select exercise"
                    :onChange="getRecommendedSets"
                    >
                    
                </v-select>
            </div>
            <div class="sets">
                <div class="form-row">
                    <div class="col-2">
                        <p class="form-control-static"><strong>Set</strong></p>
                    </div>
                    <div class="col-4">
                        <p class="form-control-static"><strong>Repetitions</strong></p>
                    </div>
                    <div class="col-5">
                        <p class="form-control-static"><strong>Weight</strong></p>
                    </div>
                </div>
                <div class="form-row" v-for="set in sets" :key="set.index">
                    <div class="col-2"><p class="form-control-static"><strong>{{set.index + 1}}</strong></p></div>
                    <div class="col-4">
                        <label for="set-reps-@set.Index" class="sr-only">Repetitions</label>
                        <input class="form-control" v-model="set.repetitions" v-bind:id="'set-reps-' + set.index" size="3" placeholder="Reps" type="number" step="1" />
                    </div>
                    <div class="col-5">
                        <label for="set-weight-@set.Index" class="sr-only">Weight</label>
                        <input class="form-control" v-model="set.weight" v-bind:id="'set-weight-' + set.index" type="number" step="0.1" min="0" size="5" placeholder="Weight" />
                    </div>
                    <div class="col-1" v-if="set==sets[sets.length - 1]">
                        <button class="btn btn-primary btn-sm form-control-static add-set" type="button" v-on:click="addSet">
                            <span class="fa fa-plus" aria-hidden="true"></span> <span class="sr-only">Add set</span></a>
                        </button>
                    </div>
                </div>
                
            </div>
            <div class="row">
                <div class="offset-8 col-4">
                   
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <button type="submit" class="btn btn-success" :disabled="(!selectedExercise) || processingActivity">
                        <span class="fa fa-check" aria-hidden="true"></span> Finish activity
                    </button>
                </div>
            </div>
        </form> 
    </div>
  </div>
</div>


<div class="card" v-if="pendingActivities && pendingActivities.length > 0">
    <div class="card-header">Pending activities</div>
    <div class="list-group">
        <button v-for="activity in pendingActivities" v-bind:key="activity.id" v-on:click="loadPendingActivity(activity)" class="list-group-item d-flex justify-content-between align-items-center">
            {{ activity.exerciseName}}
            <span class="fa fa-chevron-right float-right"></span>
        </button>
    </div>
</div>

<div class="card" v-if="activities && activities.length > 0">
    <div class="card-header">
        Completed activities
    </div>
    <div class="list-group">
        <a v-for="activity in activities" :key="activity.id" asp-action="Detail" asp-route-id="@Model.WorkoutId" asp-route-activityId="@activity.Id" class="list-group-item d-flex justify-content-between align-items-center">
            {{ activity.exerciseName}}
            <span class="fa fa-chevron-right float-right"></span>
        </a>
    </div>
</div>

</div>

<router-link :to="{name: 'Workouts'}">Back to workouts</router-link>
</div>

</template>

<script>
import vSelect from "vue-select"
import axios from 'axios'
import moment from 'moment'

export default {
  components: {
      vSelect
 },
  data () {
    return {
      errorMessage: "",
      loadingWorkout: false,
      loadingWorkoutText: "Loading workout...",
      processingActivity: false,
      workoutStartDateTime: "",
      startDateTimeIsoFormatted: "",
      error: null,
      sets: this.createDefaultSets(),
      activities: [],
      pendingActivities: [],
      isNewActivity: true,
      selectedExercise: null,
      workoutVersion: 0
    }
  },
  created () {
    console.log("Workout component created.")
    this.fetchData()
  },
   watch: {
    // call again the method if the route changes
    '$route': 'fetchData'
  },
  methods: {
    fetchData () {
      this.fetchExercises()
      this.fetchWorkout()
    },
    fetchWorkout ()  {
        this.loadingWorkout = true;
        this.loadingWorkoutText = "Loading workout...";
        var that = this;
        
        axios.get(process.env.API_ROOT + '/workouts/' + that.$route.params.workoutId)
            .then(response=>{
                var workout = response.data;
                that.setActiveWorkoutData(workout);
            })
            .catch(e=>{
                console.log(e);
                that.loadingWorkout = false;    
            })        
    },
    fetchWorkoutUntilNewer (version)  {
        axios.get(process.env.API_ROOT + '/workouts/' + this.$route.params.workoutId)
            .then(response=>{
                var workout = response.data;
                if(workout.version > version) {
                    this.setActiveWorkoutData(workout);
                } else {
                    var that = this;
                    setTimeout(function() { that.fetchWorkoutUntilNewer(version)}, 1000);
                }
            })
            .catch(e=>{
                console.log(e);
                this.loadingWorkout = false;    
            })
            
    },
    setActiveWorkoutData(workout) {
        this.pendingActivities = workout.pendingActivities;
        this.activities = workout.activities;
        this.workoutVersion = workout.version;
        this.loadingWorkout = false;
        this.startDateTimeIsoFormatted = workout.startDateTimeIsoFormatted,
        this.workoutStartDateTime = workout.startDateTime
    },
    fetchExercises () {
      console.log('Checking store.state.exercises.length:' + this.$store.state.exercises.length)
      if(this.$store.state.exercises.length == 0) {
          console.log('Dispatching exercises.')
          this.$store.dispatch('fetchExercises')
      }
    }, 
    addSet (weight, repetitions) {
        if(repetitions && weight){
            this.sets.push({repetitions:repetitions, weight: weight, index: this.sets.length});
        }
        else{
            var last = this.sets[this.sets.length - 1];
            this.sets.push({repetitions:last.repetitions, weight: last.weight, index: this.sets.length});
        }
    },
    finishActivity () {
        this.processingActivity = true;
        var exerciseId = this.selectedExercise.id
        if(this.isNewActivity){
            this.saveNewActivity(exerciseId)
        } else {
            this.updateActivity(exerciseId)
        }
    },
    updateActivity (exerciseId) {
        console.error('Not implemented')
        this.processingActivity = false
    },
    saveNewActivity (exerciseId) {
        var payload = {
                        workoutId: this.$route.params.workoutId,
                        exerciseId: exerciseId,
                        sets: this.sets,
                        version: this.workoutVersion
                    };
        axios.post(process.env.API_ROOT + '/workouts/' + payload.workoutId + '/activities', payload)
            .then(r => {
                this.fetchWorkoutUntilNewer(this.workoutVersion)
                console.log("Pending activities left: " + this.pendingActivities.length)
                this.pendingActivities = 
                    this.pendingActivities.filter(a => { 
                        console.log('Checking pending activity ' + a.exerciseName + '(' + a.exerciseId + ')')
                        var result = this.activities.find(x=>{x.exerciseId == a.exerciseId })
                        console.log(a.exerciseId + (result ? '' : ' not ') + 'found')
                        return !result
                    });
                console.log("Pending activities left after filter: " + this.pendingActivities.length)
                if(this.pendingActivities.length) {
                    var newExercise = this.findExercise(this.pendingActivities[0].exerciseId)
                    console.log('New exercise found: ' + newExercise.id)
                    this.getRecommendedSets(newExercise)
                } else { 
                    this.clearActivity() 
                }
                this.processingActivity = false
            }).catch(ex => {
                this.errorMessage = "Unable to save activity: " + ex
                console.log(ex)
                this.processingActivity = false
            })
    },
    clearActivity(){
        this.isNewActivity = true;
        this.selectedExercise = null;
        this.sets = this.createDefaultSets();
    },
    getRecommendedSets (exercise) {
        if(exercise && exercise !== this.selectedExercise) {
          this.processingActivity = true;
          var that = this
          axios.get(process.env.API_ROOT + '/my/exercises/' + exercise.id + '/last')
                .then(function (response) {
                    var data = response.data;
                    var sets = data && data.sets;
                    console.log(sets)
                    if (sets && sets.length) {
                        console.log('Mapping sets')
                        that.sets = sets.map( (set, idx) => ({index: idx, ...set}));
                        console.log('Mapped sets)')
                        console.log(that.sets)
                    } else {
                        console.log('Default sets')
                        that.sets = createDefaultSets();
                    }
                    that.selectedExercise = exercise
                    that.processingActivity = false;
                })
                .catch(e=>{
                    console.error(e)
                    that.selectedExercise = exercise
                    that.processingActivity = false;
                })
        }
    },
    loadPendingActivity(activity){
        console.log('Loading pending activity...')
        this.isNewActivity = true;
        this.getRecommendedSets(this.findExercise(activity.exerciseId));
    },
    findExercise(exerciseId){
      console.log('Finding exercise id ' + exerciseId)
      return this.$store.state.exercises.find(e=>e.id == exerciseId)  
    },
    loadCompletedActivity(activity){
        console.log('Loading completed activity...')
        this.activeActivity = activity;
    },
    createDefaultSets() { 
        return [
        { index: 0, weight: 0, repetitions: 0 },
        { index: 1, weight: 0, repetitions: 0 },
        { index: 2, weight: 0, repetitions: 0 }
        ];
    },
    copyWorkout () {    
        var payload = {
                workoutId: this.$route.params.workoutId,
                currentDateTime: moment().toISOString(),
            };
        this.loadingWorkout = true
        this.loadingWorkoutText = "Creating new workout..."
        axios.post(process.env.API_ROOT + '/workouts/' + payload.workoutId + '/copy', payload)
            .then(r => {
                this.$router.push({ name: 'Workout', params: { workoutId: r.data.workoutId } })
            })
            .catch(e=>{
                this.errorMessage = 'Error occured when trying to create new workout: ' + e
                this.loadingWorkout = false
            })
    }
  }
}

/*
    $(function () {

        $('a.submit-form').click(function () {
        $($(this).attr('data-target')).submit();
        });
        $('.hidden-submit').hide();

        $('form.copy-workout').submit(function () {
            $('form.copy-workout .current-datetime').val(moment().format())
        })

        var startDateTime = moment($("#StartDateTime-ISO").val());
        $('.startDateTimeheader').text(startDateTime);
        $('.add-set').click(function () { addSet(0, 0) });

        $('#startedDateTimeInput').datetimepicker({
            format: 'YYYY/MM/DD HH:mm'
        });
        $('#startedDateTimeInput')
            .data("DateTimePicker")
            .date(startDateTime);

        $("#exerciseSelect").selectize();

        $('#exerciseSelect').change(function (event) {
            var exerciseId = $("#exerciseSelect option:selected").val();

            fetch('/api/users/@Model.UserId/exercises/' + exerciseId + '/last')
                .then((data) => data.json())
                .then(function (data) {
                    var sets = data && data.sets;
                    if (sets && sets.length) {
                        $('.set-table-body').empty();
                        var ii = 0;
                        for (ii = 0; ii < sets.length; ii++) {
                            addSet(sets[ii].weight, sets[ii].repetitions)
                        }
                    }
                })
            })
        })
    */
</script>
<style lang="css">

</style>