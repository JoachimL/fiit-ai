using Bodybuildr.CommandStack.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bodybuildr.Domain.Workouts
{
    public class Workout : AggregateRoot
    {
        private Guid _id;
        private string _userId;
        private DateTimeOffset _startDateTime;

        public Workout() { }

        public Workout(Guid id, string userId, DateTimeOffset startDateTime)
        {
            ApplyChange(new WorkoutStarted(id, userId, startDateTime));
        }

        public override Guid Id => _id;

        public void Apply(WorkoutStarted e)
        {
            _id = e.Id;
            _startDateTime = e.StartDateTime;
            _userId = e.UserId;
        }

        public void ActivityCompleted(
            Guid exerciseId,
            IEnumerable<Set> sets, 
            int rating, 
            DateTimeOffset added)
        {
            if (sets == null || !sets.Any())
                throw new ArgumentException("At least one set must be provided", "sets");
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Invalid rating - must be between 0 and 5.", "rating");
            ApplyChange(new ActivityCompleted(_id, exerciseId, sets, rating, added));
        }
    }
}
