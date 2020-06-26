using Api.Planner.Core.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Api.Planner.Core.Commands.Forms
{
    /// <summary>
    /// Command to get application Type
    /// </summary>
    public class GetApplicationType : BaseCommand, IRequest<List<ApplicationTypeModel>>
    {
    }
}