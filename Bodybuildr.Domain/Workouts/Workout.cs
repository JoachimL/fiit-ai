using Bodybuildr.Domain.Workouts.Events;
using System;
using System.Collections.Generic;
using System.Linq;

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
            ApplyChange(new WorkoutCreated(id, userId, startDateTime));
        }

        public override Guid Id => _id;

        public void Apply(WorkoutCreated e)
        {
            _id = e.Id;
            _startDateTime = e.StartDateTime;
            _userId = e.UserId;
        }

        public void AddActivity(
            Guid activityId,
            string exerciseId,
            IEnumerable<Set> sets,
            int rating,
            DateTimeOffset added)
        {
            if (sets == null || !sets.Any())
                throw new ArgumentException("At least one set must be provided", "sets");
            if (rating < 0 || rating > 5)
                throw new ArgumentException("Invalid rating - must be between 0 and 5.", "rating");
            ApplyChange(new ActivityCompleted(
                _id,
                activityId,
                exerciseId,
                _userId,
                sets, rating, added));
        }

        public void UpdateStartTime(DateTimeOffset startDateTime)
        {
            _startDateTime = startDateTime;
            ApplyChange(new WorkoutStartDateTimeUpdated(_userId, _id, _startDateTime));
        }

        public void CopyActivitiesFromWorkout(Guid workoutToCopy, IEnumerable<Activity> activities)
        {
            ApplyChange(new ActivitiesCopiedFromWorkout(_id, _userId, workoutToCopy, activities));
        }
    }
}
