using MediatR;

namespace Api.Planner.Core.Commands.Globals
{
    /// <summary>
    /// Model to Manaege Email Request
    /// </summary>
    public class EmailRequest : IRequest<bool>
    {
        /// <summary>
        /// Gets or sets Email id.
        /// </summary>
        public string EmailId { get; set; }

        /// <summary>
        /// Gets or sets Email Type.
        /// </summary>
        public string EmailType { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }
    }
}