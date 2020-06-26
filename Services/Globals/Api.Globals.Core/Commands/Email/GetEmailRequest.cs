using MediatR;

namespace Api.Globals.Core.Commands.Email
{
    /// <summary>
    /// Class to get Email Request
    /// </summary>
    public class GetEmailRequest : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Email id
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets Email Type
        /// </summary>
        public string EmailType { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Name { get; set; }
    }
}