using MediatR;
using System.Collections.Generic;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Commands.Forms
{
    /// <summary>
    /// Class to get list of section type
    /// </summary>
    public class GetSectionType : BaseCommand, IRequest<List<SectionType>>
    {
    }
}