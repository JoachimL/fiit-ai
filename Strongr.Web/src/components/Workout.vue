<template>
  <div class="container-fluid">
      <div class="col-lg-3">
    <h1>Active workout</h1>
        <form>
            <div class="form-group">
                <label for="input">Exercise:</label>
                <input id="input" class="form-control" type="text" placeholder="Type to search...">
                <typeahead v-model="selectedExercise" target="#input" :data="exercises" item-key="name"/>
            </div>
            <div class="form-row">
                <label>Sets</label>
            </div>
            <div class="form-row" v-for="(set, index) in sets" v-bind:key="'set-' + index">
                <div class="col-xs-2">
                    <label :for="'rep-' + index">{{index}}</label>
                </div>
                <div class="col-xs-4">
                    <input :id="'rep-' + index" type="number" class="form-control" placeholder="Reps" />
                </div>
                <div class="col-xs-4">
                    <input :id="'weight-' + index" type="number" class="form-control" placeholder="Weight" v-if="selectedExercise.equipmentType != 'Body Only'" />
                </div>
                <div class="col-xs-2">
                    <button class="btn btn-danger" v-on:click="sets.pop()" v-if="index==sets.length-1 && sets.length > 1">Slett</button>
                </div>
                <div class="col-xs-12">
                <button class="btn btn-default" type="button" v-if="index==sets.length-1" v-on:click="sets.push(set)">Add set</button>
                </div>
            </div>
            <div class="form-row">
                <label>How was this activity?</label>
            </div>
            <div class="form-row">
                <div class="col-xs-4"><input type="radio" name="rating" value="easy" v-bind="difficulty" />Easy</div>
                <div class="col-xs-4"><input type="radio" name="rating" value="medium"  v-bind="difficulty" />Medium</div>
                <div class="col-xs-4"><input type="radio" name="rating" value="hard"  v-bind="difficulty" />Hard</div>
            </div>
            <div class="checkbox">
                <button class="btn btn-success" v-click="saveActivity()">Save</button>
            </div>
        </form>
    </div>
  </div>
</template>

<script>
  export default {
    data () {
      return {
        sets: [
            {},
            {}
        ],
        selectedExercise: {},
        exercises: [],
        saveActivity: function () {

        }
      }
    }
  }
</script>
<style lang="css">

</style>
