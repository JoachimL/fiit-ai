using System;
using System.Collections.Generic;
using System.Text;

namespace Bodybuildr.CommandStack.Events
{
    public class WorkoutStarted : Event
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }

        public WorkoutStarted(Guid id, string userId, DateTimeOffset startDateTime)
        {
            this.Id = id;
            this.UserId = userId;
            this.StartDateTime = startDateTime;
        }
    }
}
