using Api.FormEngine.Core.ViewModels;
using MediatR;
using System.Collections.Generic;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class to Get Applciation Type
    /// </summary>
    public class GetApplicationType : BaseCommand, IRequest<List<ApplicationTypeModel>>
    {
    }
}