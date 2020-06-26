using MediatR;
using System;

namespace Api.FormEngine.Core.Commands.Forms
{
    public class CreateApplication : BaseCommand, IRequest<Guid>
    {
        public string ApplicationName { get; set; }

        public Guid ApplicationId { get; set; }
    }
}