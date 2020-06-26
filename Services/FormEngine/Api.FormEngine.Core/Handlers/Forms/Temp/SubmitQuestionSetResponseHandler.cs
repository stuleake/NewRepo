using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Commands.Forms.Temp;
using AutoMapper;
using CT.KeyVault;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.PlanningPortal;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponseHandler : BaseHandler, IRequestHandler<SubmitQuestionSetResponsePP2, Guid>
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitQuestionSetResponseHandler"/> class
        /// </summary>
        /// <param name="keyvault">Takes IVaultManager as parameter</param>
        /// <param name="config">Takes IConfiguration as parameter</param>
        /// <param name="mediatR">Takes IMediator as parameter</param>
        /// <param name="mapper">Takes IMapper as parameter</param>
        public SubmitQuestionSetResponseHandler(IVaultManager keyvault, IConfiguration config, IMediator mediatR, IMapper mapper) : base(keyvault, config)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<Guid> Handle(SubmitQuestionSetResponsePP2 request, CancellationToken cancellationToken)
        {
            return await SubmitQuestionSetResponseAsync(request).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Guid> SubmitQuestionSetResponseAsync(SubmitQuestionSetResponsePP2 request)
        {
            PlanningPortalContext planningPortalContext = await GetContextAsync(request).ConfigureAwait(false);
            IDbContextTransaction transaction = planningPortalContext.Database.BeginTransaction();
            var errorOccurred = false;
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var application = mapper.Map<SubmitQuestionSetResponsePP2, CreateApplication>(request);
                await mediatR.Send(application).ConfigureAwait(false);
                var lobQuestionResponse = new TQ.Data.PlanningPortal.Schemas.Dbo.QuestionSetResponse
                {
                    QuestionSetId = request.QuestionSetId,
                    LastSaved = DateTime.UtcNow,
                    Response = request.Response,
                    UserApplicationId = request.ApplicationId
                };

                // Submit Data to PP DB.
                planningPortalContext.QuestionSetResponses.Add(lobQuestionResponse);
                await planningPortalContext.SaveChangesAsync().ConfigureAwait(false);

                return lobQuestionResponse.QuestionSetResponseId;
            }
            catch (Exception)
            {
                transaction.Rollback();
                errorOccurred = true;
                throw;
            }
            finally
            {
                if (!errorOccurred)
                {
                    transaction.Commit();
                }
            }
        }
    }
}