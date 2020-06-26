using MediatR;
using System;

namespace Api.PP2.Core.Commands.Forms
{
    /// <summary>
    /// Class to create Application
    /// </summary>
    public class CreateApplication : BaseCommand, IRequest<Guid>
    {
        /// <summary>
        /// Gets or Sets Application Name
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or Sets Application Id
        /// </summary>
        public Guid ApplicationId { get; set; }
    }
}