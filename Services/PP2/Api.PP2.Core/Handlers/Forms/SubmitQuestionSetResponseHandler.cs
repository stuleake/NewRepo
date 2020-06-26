using Api.PP2.Core.Commands.Forms;
using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.PlanningPortal;

namespace Api.PP2.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Submit question set response
    /// </summary>
    public class SubmitQuestionSetResponseHandler : IRequestHandler<SubmitQuestionSetResponse, Guid>
    {
        private readonly IMediator mediatR;
        private readonly IMapper mapper;
        private readonly PlanningPortalContext planningPortalContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitQuestionSetResponseHandler"/> class.
        /// </summary>
        /// <param name="mediatR">object of mediatR being passed using dependency injection</param>
        /// <param name="mapper">object of mapper being passed using dependency injection</param>
        /// <param name="planningPortalContext">object of planningPortalContext being passed using dependency injection</param>
        public SubmitQuestionSetResponseHandler(IMediator mediatR, IMapper mapper, PlanningPortalContext planningPortalContext)
        {
            this.mediatR = mediatR;
            this.mapper = mapper;
            this.planningPortalContext = planningPortalContext;
        }

        /// <inheritdoc/>
        public async Task<Guid> Handle(SubmitQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var application = mapper.Map<SubmitQuestionSetResponse, CreateApplication>(request);
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
    }
}