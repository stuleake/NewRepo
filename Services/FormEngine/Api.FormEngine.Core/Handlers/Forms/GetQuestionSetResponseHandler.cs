using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Enums;
using Api.FormEngine.Core.ViewModels;
using Api.FormEngine.Core.ViewModels.QSR;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Get Question set response
    /// </summary>
    public class GetQuestionSetResponseHandler : IRequestHandler<GetQuestionSetResponse, QuestionSetResponse>
    {
        private readonly FormsEngineContext formsEngineContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuestionSetResponseHandler"/> class
        /// </summary>
        /// <param name="formsEngineContext">object of formsEngineContext being passed using dependency injection</param>
        public GetQuestionSetResponseHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public async Task<QuestionSetResponse> Handle(GetQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // looks for the application
            var application = await formsEngineContext.QSCollection.FirstOrDefaultAsync(ua => ua.ApplicationName.ToLower() == request.ApplicationName.ToLower()
             && ua.UserId.Equals(request.UserId) && ua.CountryCode.ToLower() == request.Country.ToLower()).ConfigureAwait(false);

            if (application == null)
            {
                return null;
            }

            // if application is found, the response are taken from question set response for the application
            if (application != null && application?.QSCollectionId != null)
            {
                var qsr = await formsEngineContext.Qsr.FirstOrDefaultAsync(x => x.QSCollectionId.Equals(application.QSCollectionId)).ConfigureAwait(false);

                if (qsr == null)
                {
                    return null;
                }

                var results = (from questionSet in formsEngineContext.QS
                               join questionSetResponse in formsEngineContext.Qsr on questionSet.QSNo equals questionSetResponse.QSNo
                               join questionSetSectionMapping in formsEngineContext.QSSectionMapping on questionSet.QSId equals questionSetSectionMapping.QSId
                               join sectionFieldMapping in formsEngineContext.SectionFieldMapping on questionSetSectionMapping.SectionId equals sectionFieldMapping.SectionId
                               join questionSetResponseAnswer in formsEngineContext.QsrAnswer on
                                   new { FieldId = questionSetResponse.QsrId, FieldNo = (int)sectionFieldMapping.FieldNo } equals
                                   new { FieldId = questionSetResponseAnswer.QsrId, FieldNo = questionSetResponseAnswer.FieldNo }
                               join subQueryQuestionSetVersion in from subQs in formsEngineContext.QS
                                           join subQsr in formsEngineContext.Qsr on subQs.QSNo equals subQsr.QSNo
                                           where subQs.StatusId == (int)FormsStatus.Active && subQsr.QsrId == qsr.QsrId
                                           select subQs.QSVersion
                                    on questionSet.QSVersion equals subQueryQuestionSetVersion
                               where questionSetResponse.QsrId == qsr.QsrId
                               select new QsrAnswerModel
                               {
                                   FieldId = sectionFieldMapping.FieldId,
                                   Answer = questionSetResponseAnswer.Answer,
                                   FieldNo = (int)sectionFieldMapping.FieldNo,
                               }).ToList();

                return new QuestionSetResponse
                {
                    QuestionSetResponseId = qsr.QsrId,
                    UserApplicationId = application.QSCollectionId,
                    ApplicationName = application.ApplicationName,
                    QuestionSetId = qsr.QSId,
                    LastSaved = qsr.LastModifiedDate,
                    ModifiedBy = qsr.LastModifiedBy,
                    Response = JsonConvert.SerializeObject(results.ToDictionary(answerModelKey => $"{answerModelKey.FieldId}-{answerModelKey.FieldNo}", answerModelValue => answerModelValue.Answer))
                };
            }

            return null;
        }
    }
}