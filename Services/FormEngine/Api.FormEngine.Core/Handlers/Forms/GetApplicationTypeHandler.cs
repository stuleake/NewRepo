using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler for Getting All the Types of Applications
    /// </summary>
    public class GetApplicationTypeHandler : IRequestHandler<GetApplicationType, List<ApplicationTypeModel>>
    {
        private readonly FormsEngineContext frmEnginecontext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetApplicationTypeHandler"/> class.
        /// </summary>
        /// <param name="frmEnginecontext">Form Engine DB context Object being passed via DI</param>
        /// <param name="mapper">Mapper Object being passed via DI</param>
        public GetApplicationTypeHandler(FormsEngineContext frmEnginecontext, IMapper mapper)
        {
            this.frmEnginecontext = frmEnginecontext;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<List<ApplicationTypeModel>> Handle(GetApplicationType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            List<QSCollectionType> questionSetCollectionTypes;
            try
            {
                questionSetCollectionTypes = await frmEnginecontext.QSCollectionType.
                                            Where(qsCollectionType => qsCollectionType.CountryCode.ToLower() == request.Country.ToLower()
                                            && qsCollectionType.Product.ToLower() == request.Product.ToLower())
                                            .ToListAsync()
                                            .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var message = $"Failed to fetch Application Type : {request.Country}";
                throw new Exception($"{message}", ex);
            }

            return mapper.Map<List<QSCollectionType>, List<ApplicationTypeModel>>(questionSetCollectionTypes);
        }
    }
}