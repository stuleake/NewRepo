using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Commands.Forms.Temp;
using MediatR;
using System;
using System.Threading;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler that manages the Submit of the Question Set.
    /// </summary>
    public class SubmitQuestionTempHandler : IRequestHandler<SubmitQuestionSetResponse, bool>
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitQuestionTempHandler"/> class.
        /// </summary>
        /// <param name="mediator">mediator</param>
        public SubmitQuestionTempHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <inheritdoc/>
        public async System.Threading.Tasks.Task<bool> Handle(SubmitQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // validate
            ValidateQuestionSetResponse cmd = new ValidateQuestionSetResponse
            {
                ApplicationName = request.ApplicationName,
                QuestionSetId = request.QuestionSetId,
                QSCollectionId = request.QSCollectionId,
                Country = request.Country,
                UserId = request.UserId
            };
            var response = await this.mediator.Send(cmd).ConfigureAwait(false);

            // submit
            SubmitQuestionSetResponsePP2 pp2 = new SubmitQuestionSetResponsePP2
            {
                ApplicationId = response.UserApplicationId,
                QuestionSetId = response.QuestionSetId,
                Response = response.Response,
                Country = request.Country,
                UserId = request.UserId,
                ApplicationName = request.ApplicationName
            };
            await this.mediator.Send(pp2).ConfigureAwait(false);

            // delete
            DeleteQuestionSetResponse cmdDelete = new DeleteQuestionSetResponse
            {
                Id = response.UserApplicationId
            };
            return await this.mediator.Send(cmdDelete).ConfigureAwait(false);
        }
    }
}