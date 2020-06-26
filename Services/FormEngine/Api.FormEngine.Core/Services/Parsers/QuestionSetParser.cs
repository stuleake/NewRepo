using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.ViewModels.SheetModels;
using AutoMapper;
using System;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class QuestionSetParser : IParser<QuestionSetParserModel, QS>
    {
        private readonly IReadOnlyRepository<QS> questionSetRepository;
        private readonly IMapper mapper;

        /// <inheritdoc/>
        public QuestionSetParser(IReadOnlyRepository<QS> questionSetRepository, IMapper mapper)
        {
            this.questionSetRepository = questionSetRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public ParseResult<QS> Parse(QuestionSetParserModel model)
        {
            var sheetQuestionSet = model.QuestionSet;
            var dateTimeNow = DateTime.UtcNow;

            var availableQsDetails = questionSetRepository.GetQueryable()
                .Where(qs => qs.QSNo == Convert.ToInt32(sheetQuestionSet.QsNo))
                .OrderByDescending(orderDesc => orderDesc.QSVersion)
                .FirstOrDefault();

            var questionSet = mapper.Map<QuestionSet, QS>(sheetQuestionSet);

            questionSet.FileLocation = model.ExcelUrl;
            questionSet.LastModifiedDate = dateTimeNow;
            questionSet.LastModifiedBy = model.UserId;
            questionSet.Product = model.Product;
            questionSet.QSName = sheetQuestionSet.QSLabel;

            if (availableQsDetails != null)
            {
                if (availableQsDetails.StatusId == FormStructureConstants.ActiveQsStatusNumber)
                {
                    var avaiableQuestionSetVersion = availableQsDetails.QSVersion;
                    questionSet.CreatedDate = dateTimeNow;
                    questionSet.CreatedBy = model.UserId;
                    questionSet.StatusId = FormStructureConstants.DraftQsStatusNumber;
                    questionSet.QSVersion = avaiableQuestionSetVersion + FormStructureConstants.DraftQsStatusNumber;
                    questionSet.WarningMessage = string.Empty;

                    return new ParseResult<QS>
                    {
                        Added = questionSet
                    };
                }
                else
                {
                    questionSet.QSId = availableQsDetails.QSId;
                    availableQsDetails.QSNo = questionSet.QSNo;
                    availableQsDetails.QSName = questionSet.QSName;
                    availableQsDetails.Label = questionSet.Label;
                    availableQsDetails.Helptext = questionSet.Helptext;
                    availableQsDetails.Description = questionSet.Description;
                    availableQsDetails.LastModifiedDate = dateTimeNow;
                    availableQsDetails.LastModifiedBy = model.UserId;
                    availableQsDetails.FileLocation = questionSet.FileLocation;
                    availableQsDetails.Product = questionSet.Product;
                    availableQsDetails.Tenant = questionSet.Tenant;
                    availableQsDetails.Language = questionSet.Language;
                    availableQsDetails.WarningMessage = string.Empty;

                    return new ParseResult<QS>
                    {
                        Updated = availableQsDetails
                    };
                }
            }
            else
            {
                questionSet.CreatedDate = dateTimeNow;
                questionSet.CreatedBy = model.UserId;
                questionSet.QSVersion = FormStructureConstants.DefaultVersion;
                questionSet.StatusId = 1;

                return new ParseResult<QS>
                {
                    Added = questionSet
                };
            }
        }
    }
}