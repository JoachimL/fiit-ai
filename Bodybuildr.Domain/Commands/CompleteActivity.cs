using Bodybuildr.Domain;
using Bodybuildr.Domain.Workouts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Commands
{
    public class CompleteActivity
    {
        public Guid WorkoutId { get; set; }
        public Guid ExerciseId { get; set; }
        public Set[] Sets { get; set; }
        public int Rating { get; set; }
    }
}
