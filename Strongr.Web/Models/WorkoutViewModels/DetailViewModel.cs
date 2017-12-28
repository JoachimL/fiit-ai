using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StrongR.ReadStack.WorkoutDetail;
using StrongR.ReadStack.Workouts.TableStorage;

namespace Strongr.Web.Models.WorkoutViewModels
{
    public class DetailViewModel
    {
        public string UserId { get; set; }
        public Guid ActivityId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public string StartDateTimeIsoFormatted { get; set; }
        public ActivityListDetail[] Activities { get; internal set; }
        public Guid WorkoutId { get; internal set; }
        public IEnumerable<Exercise> AllExercises { get; internal set; }
        public Set[] Sets { get; set; }
        public int Rating { get; set; }
        public int Version { get; set; }
        public string ExerciseId { get; internal set; }

        public IEnumerable<ActivityListDetail> PendingActivities { get; set; }
    }
}
