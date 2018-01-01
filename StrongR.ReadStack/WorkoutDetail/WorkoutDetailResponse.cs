using System;

namespace StrongR.ReadStack.WorkoutDetail
{
    public class WorkoutDetailResponse
    {
        public static WorkoutDetailResponse Empty { get; } = new WorkoutDetailResponse
        {
            Activities = new ActivityListDetail[0],
            Id = Guid.Empty,
            PendingActivities = new ActivityListDetail[0],
            SelectedActivity = ActivityDetail.Empty,
            StartedDateTime = DateTimeOffset.MinValue,
            Version = -1
        };

        public DateTimeOffset StartedDateTime { get; set; }
        public ActivityListDetail[] Activities { get; set; }
        public ActivityListDetail[] PendingActivities { get; set; }
        public Guid Id { get; set; }
        public ActivityDetail SelectedActivity { get; set; }
        
        public int Version { get; set; }
    }
}