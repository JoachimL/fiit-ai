import Vue from 'vue'
import Router from 'vue-router'
import Workouts from '@/components/Workouts'
import Workout from '@/components/Workout'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'Workouts',
      component: Workouts
    },
    {
      path: '/workout',
      name: 'Workout',
      component: Workout
    }
  ]
})
