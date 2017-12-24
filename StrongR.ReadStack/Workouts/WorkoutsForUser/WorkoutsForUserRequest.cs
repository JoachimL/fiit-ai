using MediatR;

namespace StrongR.ReadStack.Workouts.WorkoutsForUser
{
    public class WorkoutsForUserRequest : IRequest<WorkoutsForUserResponse>
    {
        public WorkoutsForUserRequest(string userId)
        {
            UserId = userId;
        }

        public string UserId { get; }
    }
}
