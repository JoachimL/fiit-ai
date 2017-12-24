using MediatR;
using System;

namespace Bodybuildr.Domain.Workouts.Commands
{
    public class CompleteActivity : IRequest<CompleteActivityResponse>
    {
        public CompleteActivity(
            DateTimeOffset added, 
            Guid workoutId, 
            string exerciseId, 
            Set[] sets, int rating, int version)
        {
            Added = added;
            WorkoutId = workoutId;
            ExerciseId = exerciseId;
            Sets = sets;
            Rating = rating;
            Version = version;
        }

        public DateTimeOffset Added { get; }
        public Guid WorkoutId { get;  }
        public string ExerciseId { get; }
        public Set[] Sets { get;  }
        public int Rating { get;  }
        public int Version { get; }
    }
}
