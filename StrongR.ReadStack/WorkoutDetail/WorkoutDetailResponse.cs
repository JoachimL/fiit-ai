using System;

namespace StrongR.ReadStack.WorkoutDetail
{
    public class WorkoutDetailResponse
    {
        public DateTimeOffset StartedDateTime { get; set; }
        public ActivityListDetail[] Activities { get; set; }
        public ActivityListDetail[] PendingActivities { get; set; }
        public Guid Id { get; set; }
        public ActivityDetail SelectedActivity { get; set; }
        
        public int Version { get; set; }
    }
}