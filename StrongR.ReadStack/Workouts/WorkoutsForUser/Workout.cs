using System;
using System.Collections.Generic;
using System.Text;

namespace StrongR.ReadStack.Workouts.WorkoutsForUser
{
    public class Workout
    {
        public Guid Id { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public string UserId { get; set; }
    }
}
