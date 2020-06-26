using MediatR;

namespace Api.Globals.Core.Commands.LastLogin
{
    /// <summary>
    /// Class to get last login Request
    /// </summary>
    public class LastLoginRequest : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets the Email Id of a user
        /// </summary>
        public string EmailId { get; set; }
    }
}