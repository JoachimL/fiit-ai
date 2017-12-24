using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Workouts
{
    public class Activity
    {
        public string ExerciseId { get; set; }
        public Set[] Sets { get; set; }
        public Guid Id { get; set; }
        public string ExerciseName { get; set; }
    }
}
