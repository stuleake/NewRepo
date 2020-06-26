using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.ViewModels;
using Api.FormEngine.Core.ViewModels.QSR;
using Api.FormEngine.Core.ViewModels.QuestionSets;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Validate question set response
    /// </summary>
    public class ValidateQuestionSetResponseHandler : IRequestHandler<ValidateQuestionSetResponse, QuestionSetValidate>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateQuestionSetResponseHandler"/> class
        /// </summary>
        /// <param name="formsEngineContext">object of frmEnginecontext being passed using dependency injection</param>
        public ValidateQuestionSetResponseHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public async Task<QuestionSetValidate> Handle(ValidateQuestionSetResponse request, CancellationToken cancellationToken)
        {
            return await ValidateQuestionSetResponseAsync(request).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<QuestionSetValidate> ValidateQuestionSetResponseAsync(ValidateQuestionSetResponse request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // validate the QSR Answers
            var fieldErrorMessages = new List<FieldErrorMessage>();
            var qsrValidateData = new List<QsrValidate>();
            var answerGuideValueType = new List<AnswerGuideValueType>();
            bool validRequest = false;
            var qsrValidates = GetQsrAnswerByQsCollection(request.QSCollectionId);
            var answerGuideValues = GetQsrAnswerValues(request.QSCollectionId);

            qsrValidateData.AddRange(qsrValidates);
            if (answerGuideValues.Any())
            {
                answerGuideValueType.AddRange(answerGuideValues);
            }

            if (qsrValidateData.Count > 0)
            {
                fieldErrorMessages = Validate(qsrValidateData, answerGuideValueType).ToList();
                if (fieldErrorMessages.Count <= 0)
                {
                    validRequest = true;
                }
            }

            return new QuestionSetValidate
            {
                Status = validRequest,
                ErrorMessage = fieldErrorMessages.Any() ? fieldErrorMessages : null
            };
        }

        /// <summary>
        /// validate QSR Answer
        /// </summary>
        /// <param name="qsrResponse">QSR</param>
        /// <param name="answerGuideValueTypes">answerGuide Value Types</param>
        /// <returns>List of field error </returns>
        public static IEnumerable<FieldErrorMessage> Validate(IEnumerable<QsrValidate> qsrResponse, IEnumerable<AnswerGuideValueType> answerGuideValueTypes)
        {
            try
            {
                var fieldErrorMessages = new List<FieldErrorMessage>();
                var nextfiledId = Guid.Empty;
                if (qsrResponse != null && qsrResponse.Any())
                {
                    foreach (var qsr in qsrResponse)
                    {
                        var curfieldId = qsr.FieldId;

                        // validate constraints
                        if (qsr.DisplayConstraint.ToLower() == ConstraintTypes.Solo.ToString().ToLower() && qsr.Display.ToLower() == DisplayTypes.Required.ToString().ToLower()
                            && string.IsNullOrEmpty(qsr.Answer))
                        {
                            fieldErrorMessages.Add(new FieldErrorMessage
                            {
                                FieldId = qsr.FieldId,
                                ErrorMessage = FieldErrorMessageConstants.FieldRequired
                            });
                        }
                        else if ((qsr.DisplayConstraint.ToLower() == ConstraintTypes.Depends.ToString().ToLower() && qsr.Display.ToLower() == DisplayTypes.Required.ToString().ToLower())
                                 && ((qsr.ConstraintRule == "All" && qsr.FieldCount == qsr.FieldAnswerMatchCount && string.IsNullOrEmpty(qsr.Answer))
                                 || (qsr.ConstraintRule == "Any" && qsr.ConstraintRuleCount <= qsr.FieldAnswerMatchCount && string.IsNullOrEmpty(qsr.Answer))))
                        {
                            fieldErrorMessages.Add(new FieldErrorMessage
                            {
                                FieldId = qsr.FieldId,
                                ErrorMessage = FieldErrorMessageConstants.FieldRequired
                            });
                        }

                        bool enumExists = false;
                        bool enumTypeExists = false;

                        // validate Answer
                        if (!string.IsNullOrEmpty(qsr.Answer))
                        {
                            bool isvalid = true;
                            if (curfieldId != nextfiledId)
                            {
                                enumTypeExists = Enum.TryParse<FieldTypes>(qsr.FieldType, true, out FieldTypes fieldType);
                                if (enumTypeExists)
                                {
                                    bool isfieldvalid = true;
                                    switch (fieldType)
                                    {
                                        case FieldTypes.Number:
                                            if (!double.TryParse(qsr.Answer, out double num))
                                            {
                                                isfieldvalid = false;
                                            }
                                            break;

                                        case FieldTypes.NumberSelector:
                                            if (!int.TryParse(qsr.Answer, out int number))
                                            {
                                                isfieldvalid = false;
                                            }
                                            break;

                                        case FieldTypes.Date:
                                            if (!DateTime.TryParse(qsr.Answer, out DateTime date))
                                            {
                                                isfieldvalid = false;
                                            }
                                            break;
                                    }
                                    if (!isfieldvalid)
                                    {
                                        fieldErrorMessages.Add(new FieldErrorMessage
                                        {
                                            FieldId = qsr.FieldId,
                                            ErrorMessage = FieldErrorMessageConstants.FieldDataType
                                        });
                                    }
                                }
                            }
                            enumExists = Enum.TryParse<AnswerTypes>(qsr.AnswerType, true, out AnswerTypes answertype);
                            if (enumExists)
                            {
                                switch (answertype)
                                {
                                    case AnswerTypes.Length:
                                    case AnswerTypes.Range:
                                        if (answertype == AnswerTypes.Length)
                                        {
                                            qsr.Answer = qsr.Answer.Length.ToString();
                                        }
                                        bool isValidNumber = int.TryParse(qsr.Answer, out int number);
                                        if (!isValidNumber)
                                        {
                                            isvalid = false;
                                        }
                                        else if (!string.IsNullOrEmpty(qsr.Min) && !string.IsNullOrEmpty(qsr.Max)
                                                && Convert.ToDecimal(qsr.Answer) < Convert.ToDecimal(qsr.Min)
                                                && Convert.ToDecimal(qsr.Answer) > Convert.ToDecimal(qsr.Max))
                                        {
                                            isvalid = false;
                                        }
                                        else if (!string.IsNullOrEmpty(qsr.Min) && Convert.ToDecimal(qsr.Answer) < Convert.ToDecimal(qsr.Min))
                                        {
                                            isvalid = false;
                                        }
                                        else if (!string.IsNullOrEmpty(qsr.Max) && Convert.ToDecimal(qsr.Answer) > Convert.ToDecimal(qsr.Max))
                                        {
                                            isvalid = false;
                                        }
                                        break;

                                    case AnswerTypes.Value:
                                        bool validvalueAnswer = false;
                                        if (answerGuideValueTypes.Any())
                                        {
                                            if (string.Equals(qsr.FieldType, FieldTypes.CheckBox.ToString(), StringComparison.InvariantCultureIgnoreCase))
                                            {
                                                var answerList = JsonConvert.DeserializeObject<string[]>(qsr.Answer);
                                                var guideValue = answerGuideValueTypes.Where(val => val.Fieldid == qsr.FieldId).ToList();
                                                validvalueAnswer = answerList.All(ans => guideValue.Any(val => val.Value == ans));
                                            }
                                            else
                                            {
                                                validvalueAnswer = answerGuideValueTypes.Any(val => val.Fieldid == qsr.FieldId && val.Value == qsr.Answer);
                                            }
                                        }
                                        if (!validvalueAnswer)
                                        {
                                            isvalid = false;
                                        }
                                        break;

                                    case AnswerTypes.RegexBE:
                                        Match match = Regex.Match(qsr.Answer, qsr.Regex);
                                        if (!match.Success)
                                        {
                                            isvalid = false;
                                        }
                                        break;

                                    case AnswerTypes.Date:
                                        bool isValidDate = DateTime.TryParse(qsr.Answer, out DateTime tempAnswerdate);
                                        DateTime? minDate = null;
                                        DateTime? maxDate = null;
                                        if (isValidDate)
                                        {
                                            if (qsr.Min != null)
                                            {
                                                if (qsr.Min == "today")
                                                {
                                                    minDate = DateTime.Today;
                                                }
                                                else if (DateTime.TryParse(qsr.Min, out DateTime tempmin))
                                                {
                                                    minDate = tempmin;
                                                }
                                                else if (Guid.TryParse(qsr.Min, out Guid tempFiledId))
                                                {
                                                    minDate = Convert.ToDateTime(qsrResponse.FirstOrDefault(x => x.FieldId == tempFiledId).Answer);
                                                }
                                                else
                                                {
                                                    minDate = null;
                                                }
                                            }
                                            if (qsr.Max != null)
                                            {
                                                if (qsr.Max == "today")
                                                {
                                                    maxDate = DateTime.Today;
                                                }
                                                else if (DateTime.TryParse(qsr.Max, out DateTime tempmax))
                                                {
                                                    maxDate = tempmax;
                                                }
                                                else if (Guid.TryParse(qsr.Max, out Guid tempmaxFiledId))
                                                {
                                                    maxDate = Convert.ToDateTime(qsrResponse.FirstOrDefault(x => x.FieldId == tempmaxFiledId).Answer);
                                                }
                                                else
                                                {
                                                    maxDate = null;
                                                }
                                            }
                                            if (minDate != null && maxDate != null && tempAnswerdate < minDate && tempAnswerdate > maxDate)
                                            {
                                                isvalid = false;
                                            }
                                            else if (minDate != null && tempAnswerdate < minDate)
                                            {
                                                isvalid = false;
                                            }
                                            else if (maxDate != null && tempAnswerdate > maxDate)
                                            {
                                                isvalid = false;
                                            }
                                        }
                                        else
                                        {
                                            isvalid = false;
                                        }
                                        break;
                                }
                            }
                            if (!isvalid)
                            {
                                fieldErrorMessages.Add(new FieldErrorMessage
                                {
                                    FieldId = qsr.FieldId,
                                    ErrorMessage = qsr.Errlabel
                                });
                            }
                        }
                        nextfiledId = qsr.FieldId;
                    }
                }
                return fieldErrorMessages;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IEnumerable<AnswerGuideValueType> GetQsrAnswerValues(Guid requestQsCollectionId)
        {
            var result = from questionSetResponse in formsEngineContext.Qsr
                         join questionSetMapping in formsEngineContext.QSSectionMapping on questionSetResponse.QSId equals questionSetMapping.QSId
                         join sectionFieldMapping in formsEngineContext.SectionFieldMapping on questionSetMapping.SectionId equals sectionFieldMapping.SectionId
                         join field in formsEngineContext.Field on sectionFieldMapping.FieldId equals field.FieldId
                         join answerGuide in formsEngineContext.AnswerGuide on field.FieldId equals answerGuide.FieldId into answerGuideResult
                         from answerGuideJoin in answerGuideResult.DefaultIfEmpty()
                         where answerGuideJoin.AnswerTypeId == (int)AnswerTypes.Value && questionSetResponse.QSCollectionId == requestQsCollectionId
                         select new AnswerGuideValueType
                         {
                             Fieldid = field.FieldId,
                             AnswerGuideId = answerGuideJoin.AnswerGuideId,
                             Value = answerGuideJoin.Value
                         };

            return result;
        }

        private IEnumerable<QsrValidate> GetQsrAnswerByQsCollection(Guid requestQsCollectionId)
        {
            var fieldAndAnswerJoin = (from fieldConstraint in formsEngineContext.FieldConstraints
                                      join sectionFieldMapping in formsEngineContext.SectionFieldMapping on fieldConstraint.FieldId equals sectionFieldMapping.FieldId
                                      join questionSetFieldMapping in formsEngineContext.QSSectionMapping on sectionFieldMapping.SectionId equals questionSetFieldMapping.SectionId
                                      join questionSetResponse in formsEngineContext.Qsr on questionSetFieldMapping.QSId equals questionSetResponse.QSId
                                      join answerGuide in formsEngineContext.AnswerGuide on fieldConstraint.DependantAnswerGuideId equals answerGuide.AnswerGuideId into answerGuideResult
                                      from answerGuideJoin in answerGuideResult.DefaultIfEmpty()
                                      join questionSerResponseAnswer in formsEngineContext.QsrAnswer on
                                          new { answerGuideJoin.FieldId, questionSetResponse.QsrId } equals
                                          new { questionSerResponseAnswer.FieldId, questionSerResponseAnswer.QsrId } into qsrAnswersResult
                                      from questionSetResponseAnswerJoin in qsrAnswersResult.DefaultIfEmpty()
                                      select new
                                      {
                                          FieldId = fieldConstraint.FieldId,
                                          Answer = questionSetResponseAnswerJoin.Answer,
                                          QsrId = (Guid?)questionSetResponseAnswerJoin.QsrId,
                                          DependentFieldId = answerGuideJoin.FieldId,
                                          Value = answerGuideJoin.Value
                                      }).Distinct().ToList();


            var countsJoin = (from fr in fieldAndAnswerJoin
                             group fr by fr.FieldId into grp
                             select new
                             {
                                 FieldId = grp.Key,
                                 Fieldcount = grp.Count(),
                                 FieldAnswerMatchCount = grp.Count(x => x.Answer == x.Value && x.Answer != null)
                             }).ToList();

            var resultsJoin = (from questionSetCollection in formsEngineContext.QSCollection
                               join questionSetResponse in formsEngineContext.Qsr on questionSetCollection.QSCollectionId equals questionSetResponse.QSCollectionId
                               join questionSetSectionMapping in formsEngineContext.QSSectionMapping on questionSetResponse.QSId equals questionSetSectionMapping.QSId
                               join sectionFieldMapping in formsEngineContext.SectionFieldMapping on questionSetSectionMapping.SectionId equals sectionFieldMapping.SectionId
                               join field in formsEngineContext.Field on sectionFieldMapping.FieldId equals field.FieldId
                               join questionSetResponseAnswer in formsEngineContext.QsrAnswer on
                                   new { field.FieldId, questionSetResponse.QsrId } equals
                                   new { questionSetResponseAnswer.FieldId, questionSetResponseAnswer.QsrId }
                               join answerGuide in formsEngineContext.AnswerGuide on field.FieldId equals  answerGuide.FieldId into answerGuideResult
                               from answerGuideJoin in answerGuideResult.DefaultIfEmpty()
                               join fieldType in formsEngineContext.FieldTypes on field.FieldTypeId equals fieldType.FieldTypeId into fieldTypeResult
                               from fieldTypeJoin in fieldTypeResult.DefaultIfEmpty()
                               join display in formsEngineContext.Displays on field.DisplayId equals display.DisplayId into displayResult
                               from displayJoin in displayResult.DefaultIfEmpty()
                               join constraint in formsEngineContext.Constraints on field.DisplayConstraintId equals constraint.ConstraintId into constResult
                               from constJoin in constResult.DefaultIfEmpty()
                               join rules in formsEngineContext.Rules on field.ConstraintRuleId equals rules.RuleId into rulesResult
                               from rule1Join in rulesResult.DefaultIfEmpty()
                               join constRule in formsEngineContext.Rules on field.ConstraintRuleId equals constRule.RuleId into constRuleResult
                               from constRuleJoin in constRuleResult.DefaultIfEmpty()
                               join answerRule in formsEngineContext.Rules on field.AnswerRuleId equals answerRule.RuleId into answerRuleResult
                               from answerRuleJoin in answerRuleResult.DefaultIfEmpty()
                               join answerType in formsEngineContext.AnswerTypes on answerGuideJoin.AnswerTypeId equals answerType.AnswerTypeId into answerTypeResult
                               from answerTypeJoin in answerTypeResult.DefaultIfEmpty()
                               where questionSetCollection.QSCollectionId == requestQsCollectionId && answerGuideJoin.AnswerTypeId != (int)AnswerTypes.Regex
                               select new
                               {
                                   FieldId = field.FieldId,
                                   FieldNo = field.FieldNo,
                                   QsrAnswerId = questionSetResponseAnswer.QsrAnswerId,
                                   QsrId = questionSetResponse.QsrId,
                                   Answer = questionSetResponseAnswer.Answer,
                                   FieldType = fieldTypeJoin.FieldTypes,
                                   Display = displayJoin.Displays,
                                   DisplayConstraint = constJoin.Constraints,
                                   ConstraintRule = constRuleJoin.Rules,
                                   ConstraintRuleCount = field.ConstraintRuleCount ?? default,
                                   AnswerRule = answerRuleJoin.Rules,
                                   AnswerRuleCount = field.AnswerRuleCount ?? default,
                                   AnswerType = answerTypeJoin.AnswerTypes,
                                   Errlabel = answerGuideJoin.ErrLabel,
                                   Min = answerGuideJoin.Min,
                                   Max = answerGuideJoin.Max,
                                   Regex = answerTypeJoin.AnswerTypeId == (int)AnswerTypes.RegexBE ? answerGuideJoin.Value : null
                               }).Distinct().ToList();


            var finalResult = (from result in resultsJoin
                join partTwo in countsJoin on result.FieldId equals partTwo.FieldId ?? default into resultsJoinResult
                from finalResultsJoin in resultsJoinResult.DefaultIfEmpty()
                select new QsrValidate
                {
                    FieldId = result.FieldId,
                    FieldNo = result.FieldNo,
                    QsrAnswerId = result.QsrAnswerId,
                    QsrId = result.QsrId,
                    Answer = result.Answer,
                    FieldType = result.FieldType,
                    Display = result.Display,
                    DisplayConstraint = result.DisplayConstraint,
                    ConstraintRule = result.ConstraintRule,
                    ConstraintRuleCount = result.ConstraintRuleCount,
                    AnswerRule = result.AnswerRule,
                    AnswerRuleCount = result.AnswerRuleCount,
                    AnswerType = result.AnswerType,
                    Errlabel = result.Errlabel,
                    Min = result.Min,
                    Max = result.Max,
                    Regex = result.Regex
                }).ToList();

            return finalResult;
        }
    }
}