using MediatR;

namespace Api.Planner.Core.Commands.ActivateUser
{
    public class ActivateUserRequest : IRequest<bool>
    {
        public string EmailId { get; set; }
    }
}