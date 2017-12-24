using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Strongr.Web.Models.WorkoutViewModels
{
    public class ActivityModel
    {
        public Guid WorkoutId { get; set; }
        public string ExerciseId { get; set; }
        public Guid ActivityId { get; internal set; }
        public Set[] Sets { get; set; }
        public int Rating { get; set; }
        public int Version { get; set; }
    }
}
