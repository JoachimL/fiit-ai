﻿using Bodybuildr.Domain.Workouts;
using System;
using System.Collections.Generic;

namespace Bodybuildr.Domain.Workouts.Events
{
    public class ActivityCompleted : Event
    {
        public ActivityCompleted(
            Guid workoutId,
            Guid activityId,
            string exerciseId,
            string userId,
            IEnumerable<Set> sets,
            int rating,
            DateTimeOffset added)
        {
            WorkoutId = workoutId;
            ActivityId = activityId;
            ExerciseId = exerciseId;
            Sets = sets;
            Rating = rating;
            Added = added;
            UserId = userId;
        }

        private List<Set> _sets;
        public int Rating { get; }
        public Guid ActivityId { get; }
        public string ExerciseId { get; }
        public DateTimeOffset Added { get; }
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

        public Guid WorkoutId { get; }
        
        public string UserId { get; }
    }
}
