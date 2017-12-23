using Bodybuildr.Domain.Workouts.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NodaTime;
using Strongr.Web.Commands.CopyWorkout;
using Strongr.Web.Models;
using Strongr.Web.Models.WorkoutViewModels;
using StrongR.ReadStack.TableStorage;
using StrongR.ReadStack.WorkoutDetail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly ExerciseRepository _exerciseRepository;

        public WorkoutController(
            UserManager<ApplicationUser> userManager,
            IMediator mediator,
            ExerciseRepository exerciseRepository)
        {
            _userManager = userManager;
            _mediator = mediator;
            _exerciseRepository = exerciseRepository;
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

            ZonedDateTime zonedTime = GetZonedTime(model.StartDateTime.Value, model.TimeZoneName);
            var workoutId = Guid.NewGuid();
            await _mediator.Send(
                new StartWorkout(
                    workoutId,
                    _userManager.GetUserId(User),
                    zonedTime.ToDateTimeOffset()));
            return RedirectToAction("Detail", new { id = workoutId });
        }

        private static ZonedDateTime GetZonedTime(DateTimeOffset dateTime, string timeZoneName)
        {
            var localTime = LocalDateTime.FromDateTime(dateTime.DateTime);
            var zonedTime = localTime.InZoneStrictly(DateTimeZoneProviders.Tzdb[timeZoneName]);
            return zonedTime;
        }

        public async Task<IActionResult> Detail([Required]Guid? id, Guid? activityId)
        {
            var workout = await _mediator.Send(
                new WorkoutDetailRequest(
                    id.Value,
                    _userManager.GetUserId(User),
                    activityId ?? Guid.Empty));
            var model = new DetailViewModel
            {
                StartDateTime = workout.StartedDateTime,
                Activities = workout.Activities,
                WorkoutId = workout.Id,
                ExerciseId = workout.SelectedActivity.ExerciseId,
                PendingActivities = workout.PendingActivities,
                AllExercises = await GetExercises(),
                ActivityId = workout.SelectedActivity.Id,
                Rating = workout.SelectedActivity.Rating,
                Sets = workout.SelectedActivity.Sets?.Select(s => new Set { Repetitions = s.Repetitions, Weight = s.Weight }).ToArray(),
                Version = workout.Version,
                UserId = _userManager.GetUserId(User)
            };
            return View(model);
        }

        private async Task<IEnumerable<StrongR.ReadStack.Workouts.TableStorage.Exercise>> GetExercises()
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            return exercises.OrderBy(e => e.Name);
        }

        public async Task<IActionResult> SaveActivity(ActivityModel model)
        {
            if (model.ActivityId == Guid.Empty)
            {
                var response = await _mediator.Send(
                    new CompleteActivity(
                    DateTimeOffset.Now,
                    model.WorkoutId,
                    model.ExerciseId,
                    CreateSets(model), model.Rating, model.Version));
                return RedirectToAction("Detail", new { id = model.WorkoutId });
            }
            else throw new NotImplementedException();
        }

        public async Task<IActionResult> SaveWorkout(SaveWorkoutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            await _mediator.Send(new UpdateStartDateTime(model.WorkoutId.Value, model.StartDateTime.Value, model.Version.Value));
            return RedirectToAction("Detail", new { id = model.WorkoutId });
        }

        private static Bodybuildr.Domain.Workouts.Set[] CreateSets(ActivityModel model)
        {
            return model.Sets.Where(s => s.Repetitions > 0 && s.Weight > 0).Select(s => new Bodybuildr.Domain.Workouts.Set { Repetitions = s.Repetitions, Weight = s.Weight, SystemOfMeasurement = Bodybuildr.SystemOfMeasurement.Metric }).ToArray();
        }

        public async Task<IActionResult> CopyWorkout(CopyWorkoutModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ZonedDateTime zonedTime = GetZonedTime(model.CurrentDateTime.Value, model.TimeZoneName);
            var workoutId = Guid.NewGuid();
            await _mediator.Send(
                new CopyWorkoutRequest(
                    workoutId,
                    model.WorkoutId.Value,
                    _userManager.GetUserId(User),
                    zonedTime.ToDateTimeOffset()));
            return RedirectToAction("Detail", new { id = workoutId });
        }
    }
}
