using MediatR;
using System;

namespace Bodybuildr.Domain.Workouts.Commands
{
    public class DeleteWorkout : Command, IRequest
    {
        public Guid WorkoutId { get; }
        private string UserId { get; }
        public int OriginalVersion { get; }

        public DeleteWorkout(Guid workoutId, string userid, int originalVersion)
        {
            this.WorkoutId = workoutId;
            this.UserId = userid;
            OriginalVersion = originalVersion;
        }

        
    }
}
