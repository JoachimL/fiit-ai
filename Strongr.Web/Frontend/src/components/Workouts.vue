<template>

  <div class="container-fluid">
    <h1>Your workouts</h1>

    <b-modal ref="startNewWorkoutModal" title="Start new workout" @ok="startNewWorkout">
        <p class="my-4">Start new workout with this start date and time:</p>
        <div class="input-group">
          <date-picker v-model="newWorkoutDate" :config="config" required></date-picker>
          <span class="input-group-append">
            <span class="fa fa-calendar"></span>
          </span>
        </div>
    </b-modal>

    <div>
      <button class="btn btn-success" type="button" @click="$refs.startNewWorkoutModal.show()">New workout</button>
    </div>

    <div v-if="$store.state.isLoadingWorkouts" class="text-center">
        <img src="/static/images/spinner.gif" height="256" width="256"/>
        <p class="text-muted">Loading workouts...</p>
    </div>

    <div class="list-group">
       <router-link :to="{ name: 'Workout', params: { 'workoutId': workout.id } }" 
            v-for="workout in $store.state.workouts" v-bind:key="workout.id" 
            class="list-group-item d-flex justify-content-between align-items-center">
            {{ workout.displayStartDateAndTime}}
            <span class="fa fa-chevron-right float-right"></span>
       </router-link>
    </div>
  </div>
</template>

<script>
import Vue from 'vue'
import datePicker from 'vue-bootstrap-datetimepicker'
import VueMoment from 'vue-moment'
import moment from 'moment-timezone'
import 'eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js'
import axios from 'axios'
import VueAxios from 'vue-axios'

Vue.use(VueAxios, axios)
Vue.use(VueMoment, {
    moment,
})

export default {
  name: 'Bodybuildr',
  data () {
    return {
      showNewWorkoutModal: false,
      newWorkoutDate: moment(),
      workouts: [],
      config: {
        format: 'DD/MM/YYYY HH:mm'
      }
    }
  },
  created () {
    this.fetchWorkouts()
  },
  methods: {
    startNewWorkout () {

    },
    fetchWorkouts () {
      if(this.$store.state.workouts.length == 0){
        console.log('Dispatching fetch workouts')
        this.$store.dispatch('fetchWorkouts')
      }
    }
  },
  components: {
    datePicker
  }
}
</script>

<style scoped>
@import 'eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.css';

.modal-mask {
  position: fixed;
  z-index: 9998;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .5);
  display: table;
  transition: opacity .3s ease;
}

.modal-wrapper {
  display: table-cell;
  vertical-align: middle;
}

.modal-container {
  width: 80%;
  max-width: 500px;
  margin: 0px auto;
  padding: 20px 30px;
  background-color: #fff;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
  transition: all .3s ease;
  font-family: Helvetica, Arial, sans-serif;
}

.modal-header h3 {
  margin-top: 0;
  color: #42b983;
}

.modal-body {
  margin: 20px 0;
}

.modal-default-button {
  float: right;
}

/*
 * The following styles are auto-applied to elements with
 * transition="modal" when their visibility is toggled
 * by Vue.js.
 *
 * You can easily play with the modal transition by editing
 * these styles.
 */

.modal-enter {
  opacity: 0;
}

.modal-leave-active {
  opacity: 0;
}

.modal-enter .modal-container,
.modal-leave-active .modal-container {
  -webkit-transform: scale(1.1);
  transform: scale(1.1);
}
</style>
