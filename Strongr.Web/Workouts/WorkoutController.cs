using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Strongr.Web.Models;
using Strongr.Web.Models.WorkoutViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Strongr.Web.Workouts
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly WorkoutsOrchestrator _workoutsOrchestrator;

        public WorkoutController(
            UserManager<ApplicationUser> userManager,
            WorkoutsOrchestrator workoutsOrchestrator)
        {
            _userManager = userManager;
            _workoutsOrchestrator = workoutsOrchestrator;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);
            Guid workoutId = await _workoutsOrchestrator.CreateNewWorkout(model, userId);
            return RedirectToAction("Detail", new { id = workoutId });
        }



        public async Task<IActionResult> Detail(
            [Required]Guid? id,
            Guid? activityId,
            Guid? pendingActivityId)
        {
            DetailViewModel model =
                await _workoutsOrchestrator.GetWorkoutDetails(
                    _userManager.GetUserId(User),
                    id,
                    activityId,
                    pendingActivityId);
            return View(model);
        }

        public async Task<IActionResult> SaveActivity(ActivityModel model)
        {
            var userId = _userManager.GetUserId(User);
            await _workoutsOrchestrator.CompleteActivity(model, userId);
            return RedirectToAction("Detail", new { id = model.WorkoutId });
        }

        public async Task<IActionResult> SaveWorkout(SaveWorkoutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var userId = _userManager.GetUserId(User);
            await SaveWorkout(model, userId);
            return RedirectToAction("Detail", new { id = model.WorkoutId });
        }

        private Task SaveWorkout(SaveWorkoutModel model, string userId)
            => _workoutsOrchestrator.SaveWorkout(userId, model);

        public async Task<IActionResult> CopyWorkout(CopyWorkoutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userId = _userManager.GetUserId(User);
            var newWorkoutId = await _workoutsOrchestrator.CopyWorkout(model, userId);
            return RedirectToAction("Detail", new { id = newWorkoutId });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var workout = await _workoutsOrchestrator.GetWorkoutDetails(
                _userManager.GetUserId(User),
                    id,
                    Guid.Empty,
                    Guid.Empty);
            if (workout == null)
                return BadRequest();
            return View(new DeleteViewModel
            {
                DateTimeOffset = workout.StartDateTime,
                WorkoutId = workout.WorkoutId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id, string confirm)
        {
            await _workoutsOrchestrator.DeleteWorkout(id, _userManager.GetUserId(User));
            return RedirectToAction("Index", "Home");
        }
    }
}
