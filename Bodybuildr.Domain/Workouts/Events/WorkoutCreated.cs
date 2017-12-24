using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.Domain.Workouts.Events
{
    public class WorkoutCreated : Event
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }

        public WorkoutCreated(Guid id, string userId, DateTimeOffset startDateTime)
        {
            this.Id = id;
            this.UserId = userId;
            this.StartDateTime = startDateTime;
        }
    }
}
