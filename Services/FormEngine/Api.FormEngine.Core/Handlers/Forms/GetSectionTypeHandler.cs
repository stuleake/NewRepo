using Api.FormEngine.Core.Commands.Forms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to get section type
    /// </summary>
    public class GetSectionTypeHandler : IRequestHandler<GetSectionType, List<SectionType>>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSectionTypeHandler"/> class
        /// </summary>
        /// <param name="formsEngineContext">object of frmEnginecontext being passed using dependency injection</param>
        public GetSectionTypeHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public async Task<List<SectionType>> Handle(GetSectionType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return await formsEngineContext.SectionTypes.ToListAsync().ConfigureAwait(false);
        }
    }
}