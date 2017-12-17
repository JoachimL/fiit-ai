using Bodybuildr.Domain.Workouts;
using System;
using System.Collections.Generic;

namespace Bodybuildr.CommandStack.Events
{
    public class ActivityCompleted : Event
    {
        public ActivityCompleted(
            Guid workoutId,
            Guid exerciseId,
            IEnumerable<Set> sets,
            int rating,
            DateTimeOffset added)
        {
            Id = workoutId;
            ExerciseId = exerciseId;
            Sets = sets;
            Rating = rating;
            Added = added;
        }

        private List<Set> _sets;
        private int Rating { get; }
        public Guid Id { get; set; }
        public Guid ExerciseId { get; }
        public DateTimeOffset Added { get; set; }
        public IEnumerable<Set> Sets
        {
            get
            {
                if (_sets == null)
                    _sets = new List<Set>();
                return _sets;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _sets = new List<Set>(value);
            }
        }

    }
}
