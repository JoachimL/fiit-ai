using System;
using System.Collections.Generic;
using MediatR;

namespace Bodybuildr.Domain.Workouts.Commands
{
    public class CopyActivitiesFromWorkout : Command, IRequest
    {

        public CopyActivitiesFromWorkout(Guid newWorkoutId, string userId, Guid workoutToCopy, IEnumerable<Activity> activities, int version)
        {
            TargetWorkoutId = newWorkoutId;
            UserId = userId;
            WorkoutToCopy = workoutToCopy;
            Activities = activities;
            Version = version;
        }

        public Guid TargetWorkoutId { get; }
        public string UserId { get; }
        public Guid WorkoutToCopy { get; }
        public IEnumerable<Activity> Activities { get; }
        public int Version { get; }
    }
}
