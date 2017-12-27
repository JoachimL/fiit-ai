using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Strongr.Web.Models;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WorkoutsOrchestrator _workoutsOrchestrator;

        public WorkoutsApiController(
            UserManager<ApplicationUser> userManager,
            WorkoutsOrchestrator workoutsOrchestrator)
        {
            _userManager = userManager;
            _workoutsOrchestrator = workoutsOrchestrator;
        }

        [Route("{workoutId}")]
        [HttpGet]
        public async Task<IActionResult> GetWorkoutDetails([Required]Guid? workoutId)
        {
            var workout = await _workoutsOrchestrator.GetWorkoutDetails(_userManager.GetUserId(User), workoutId, null, null);
            if (workout == null)
                return NotFound();
            return Ok(workout);
        }
    }
}
