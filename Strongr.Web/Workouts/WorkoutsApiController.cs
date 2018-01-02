using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Strongr.Web.Authentication;
using Strongr.Web.Models;
using Strongr.Web.Models.Workouts;
using Strongr.Web.Models.WorkoutViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Workouts
{
    [Route("api/workouts")]
    public class WorkoutsApiController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly WorkoutsOrchestrator _workoutsOrchestrator;

        public WorkoutsApiController(
            IUserManager userManager,
            WorkoutsOrchestrator workoutsOrchestrator)
        {
            _userManager = userManager;
            _workoutsOrchestrator = workoutsOrchestrator;
        }

        [Route("{workoutId}")]
        [HttpGet]
        public async Task<IActionResult> GetWorkoutDetails([Required]Guid? workoutId)
        {
            var workout = await _workoutsOrchestrator.GetWorkoutDetails(
                _userManager.GetUserId(User), workoutId, null, null);
            if (workout == null)
                return NotFound();
            return Ok(workout);
        }

        [HttpPost]
        [Route("{workoutId}/activities")]
        public async Task<IActionResult> AddActivity(Guid workoutId, [FromBody]ActivityModel model)
        {
            var userId = _userManager.GetUserId(User);
            model.ActivityId = Guid.Empty; // force new activity
            model.WorkoutId = workoutId;
            var response = await _workoutsOrchestrator.CompleteActivity(model, userId);
            return Ok(response);
        }

        [HttpPost]
        [Route("{workoutId}/copy")]
        public async Task<IActionResult> CopyWorkout(Guid workoutId, [FromBody]CopyWorkoutModel model)
        {
            var userId = _userManager.GetUserId(User);
            model.WorkoutId = workoutId;
            var newWorkoutId = await _workoutsOrchestrator.CopyWorkout(model, userId);
            return Ok(new { workoutId = newWorkoutId });
        }

        [HttpGet]
        [Route("")]
        public async Task<WorkoutsResponseModel> GetWorkouts()
        {
            var response = await _workoutsOrchestrator.GetWorkoutsForUser(User);
            return new WorkoutsResponseModel
            {
                Workouts = response.Workouts.Select(w =>
                    new Workout
                    {
                        Id = w.Id,
                        StartDateAndTime = w.StartDateTime,
                        DisplayStartDateAndTime = w.StartDateTime.ToString("G")
                    }).ToArray()
            };
        }
    }
}
