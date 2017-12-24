using Bodybuildr.Domain.Workouts;
using Bodybuildr.Domain.Workouts.Commands;
using MediatR;
using StrongR.ReadStack.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Strongr.Web.Commands.CopyWorkout
{
  public class CopyWorkoutRequestHandler : IRequestHandler<CopyWorkoutRequest, CopyWorkoutResponse>
  {
    private readonly IMediator _mediator;
    private readonly ActivityTableHandler _activityTableHandler;

    public CopyWorkoutRequestHandler(IMediator mediator, ActivityTableHandler activityTableHandler)
    {
      _mediator = mediator;
      this._activityTableHandler = activityTableHandler;
    }

    public async Task<CopyWorkoutResponse> Handle(CopyWorkoutRequest request, CancellationToken cancellationToken)
    {
      await _mediator.Send(new StartWorkout(request.WorkoutId, request.UserId, request.StartDateTime));
      var activities = await _activityTableHandler.RetrieveActivitiesForWorkoutAsync(request.WorkoutToCopy);
      await _mediator.Send(
        new CopyActivitiesFromWorkout(
          request.WorkoutId,
          request.UserId,
          request.WorkoutToCopy,
          activities.OrderBy(a => a.Added).Select(a => new Activity
          {
            ExerciseId = a.ExerciseId,
            Sets = a.Sets,
            Id = Guid.NewGuid()
          }), 1));
      return new CopyWorkoutResponse { WorkoutId = request.WorkoutId };
    }
  }
}
