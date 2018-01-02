// store.js
import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'

Vue.use(Vuex)

export function createStore () {
  return new Vuex.Store({
    state: {
      isLoadingExercises: false,
      exercises: [],
      isLoadingWorkouts: false,
      workouts: []
    },
    actions: {
      fetchWorkouts ({commit, state}) {
        axios
            .get(process.env.API_ROOT + '/workouts/')
            .then(response => {
                console.log(response)
                commit('setWorkouts', response.data.workouts)
                commit('finishedLoadingWorkouts')

            })
            .catch(e => {
                console.log('Error occured: ' + e)
                commit('finishedLoadingWorkouts')        
            })  
      },
      fetchExercises ({commit, state}) {
        commit('startedLoadingExercises')
        console.log('Getting /exercises')
        axios
            .get(process.env.API_ROOT + '/exercises')
            .then(exercises => {
                console.log('Got exercises')
                commit('setExercises', exercises.data.map( (e,index) => ({index, ...e})));
                commit('finishedLoadingExercises')
            })
            .catch(e => {
                commit('finishedLoadingExercises')
                console.log('Error occured: ' + e)
            })
      },

    },
    mutations: {
      startedLoadingExercises (state) {
        state.isLoadingExercises = true
      },
      finishedLoadingExercises (state) {
        state.isLoadingExercises = false
      },
      startedLoadingWorkouts (state) {
        state.isLoadingWorkouts = true
      },
      finishedLoadingWorkouts (state) {
        state.isLoadingWorkouts = false
      },
      setWorkouts(state, workouts) {
        state.workouts = workouts
      },
      setExercises (state, exercises) {
        console.log('Setting exercises')
        state.exercises = exercises
      }
    }
  })
}