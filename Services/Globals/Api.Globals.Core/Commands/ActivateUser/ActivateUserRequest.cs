using MediatR;

namespace Api.Globals.Core.Commands.ActivateUser
{
    public class ActivateUserRequest : IRequest<bool>
    {
        public string EmailId { get; set; }
    }
}