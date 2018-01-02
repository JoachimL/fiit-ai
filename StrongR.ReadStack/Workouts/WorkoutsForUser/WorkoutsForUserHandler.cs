using MediatR;
using Microsoft.WindowsAzure.Storage.Table;
using StrongR.ReadStack.TableStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrongR.ReadStack.Workouts.WorkoutsForUser
{
    public class WorkoutsForUserHandler : IRequestHandler<WorkoutsForUserRequest, WorkoutsForUserResponse>
    {
        private readonly WorkoutTableHandler _workoutTableHandler;
        
        public WorkoutsForUserHandler(WorkoutTableHandler handler)
        {
            _workoutTableHandler = handler;
        }

        public async Task<WorkoutsForUserResponse> Handle(
            WorkoutsForUserRequest request, CancellationToken cancellationToken)
        {
            var workouts =
                await _workoutTableHandler.GetWorkoutsForUserAsync(request.UserId);
            return new WorkoutsForUserResponse
            {
                Workouts = workouts.Select(w =>
                    new Workout
                    {
                        UserId = w.PartitionKey,
                        Id = Guid.Parse(w.RowKey),
                        StartDateTime = w.StartDateTime
                    }).OrderByDescending(w=>w.StartDateTime)
            };
        }
    }
}
