using Bodybuildr.Domain.Workouts.Commands;
using MediatR;
using NodaTime;
using Strongr.Web.Commands.CopyWorkout;
using Strongr.Web.Models.WorkoutViewModels;
using StrongR.ReadStack.TableStorage;
using StrongR.ReadStack.WorkoutDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Workouts
{
    public class WorkoutsOrchestrator
    {
        private readonly IMediator _mediator;
        private readonly ExerciseRepository _exerciseRepository;

        public WorkoutsOrchestrator(IMediator mediator, ExerciseRepository exerciseRepository)
        {
            _mediator = mediator;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<Guid> CreateNewWorkout(CreateViewModel model, string userId)
        {
            ZonedDateTime zonedTime = GetZonedTime(model.StartDateTime.Value, model.TimeZoneName);
            var workoutId = Guid.NewGuid();
            await _mediator.Send(
                new StartWorkout(
                    workoutId,
                    userId,
                    zonedTime.ToDateTimeOffset()));
            return workoutId;
        }

        private static ZonedDateTime GetZonedTime(DateTimeOffset dateTime, string timeZoneName)
        {
            var localTime = LocalDateTime.FromDateTime(dateTime.DateTime);
            var zonedTime = localTime.InZoneStrictly(DateTimeZoneProviders.Tzdb[timeZoneName]);
            return zonedTime;
        }

        public async Task<DetailViewModel> GetWorkoutDetails(
            string userId,
            Guid? id, Guid? activityId, Guid? pendingActivityId)
        {
            var workout = await _mediator.Send(
                            new WorkoutDetailRequest(
                                id.Value,
                                userId,
                                activityId ?? Guid.Empty,
                                pendingActivityId ?? Guid.Empty));
            var model = new DetailViewModel
            {
                StartDateTime = workout.StartedDateTime,
                StartDateTimeIsoFormatted = workout.StartedDateTime.ToString("o"),
                Activities = workout.Activities,
                WorkoutId = workout.Id,
                ExerciseId = workout.SelectedActivity.ExerciseId,
                PendingActivities = workout.PendingActivities.Where(a => a.ExerciseId != workout.SelectedActivity.ExerciseId && workout.Activities.All(x => x.ExerciseId != a.ExerciseId)),
                AllExercises = await GetExercises(),
                ActivityId = workout.SelectedActivity.Id,
                Rating = workout.SelectedActivity.Rating,
                Sets = workout.SelectedActivity.Sets?.Select(s => new Set { Repetitions = s.Repetitions, Weight = s.Weight }).ToArray(),
                Version = workout.Version,
                UserId = userId
            };
            return model;
        }

        public Task SaveWorkout(string userId, SaveWorkoutModel model)
            => _mediator.Send(new UpdateStartDateTime(userId, model.WorkoutId.Value, model.StartDateTime.Value, model.Version.Value));

        private async Task<IEnumerable<StrongR.ReadStack.Workouts.TableStorage.Exercise>> GetExercises()
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            return exercises.OrderBy(e => e.Name);
        }

        public async Task<Guid> CopyWorkout(CopyWorkoutModel model, string userId)
        {
            ZonedDateTime zonedTime = GetZonedTime(model.CurrentDateTime.Value, model.TimeZoneName);
            var workoutId = Guid.NewGuid();
            await _mediator.Send(
                              new CopyWorkoutRequest(
                                  workoutId,
                                  model.WorkoutId.Value,
                                  userId,
                                  zonedTime.ToDateTimeOffset()));
            return workoutId;
        }

        public async Task<Guid> CompleteActivity(ActivityModel model, string userId)
        {
            if (model.ActivityId == Guid.Empty)
            {
                var response = await _mediator.Send(
                    new CompleteActivity(
                        userId,
                    DateTimeOffset.Now,
                    model.WorkoutId,
                    model.ExerciseId,
                    CreateSets(model), model.Rating, model.Version));
                return response.ActivityId;
            }
            else throw new NotImplementedException();
        }

        public async Task DeleteWorkout(Guid id, string userId)
        {
            var workout = await _mediator.Send(
                new WorkoutDetailRequest(
                    id,
                    userId,
                    Guid.Empty,
                    Guid.Empty));
            if (workout == null)
                throw new ArgumentException("Invalid workout.", "id");
            await _mediator.Send(new DeleteWorkout(id, userId, workout.Version));
        }

        private static Bodybuildr.Domain.Workouts.Set[] CreateSets(ActivityModel model)
        {
            return model.Sets.Where(s => s.Repetitions > 0).Select(s => new Bodybuildr.Domain.Workouts.Set { Repetitions = s.Repetitions, Weight = s.Weight, SystemOfMeasurement = Bodybuildr.SystemOfMeasurement.Metric }).ToArray();
        }
    }
}
