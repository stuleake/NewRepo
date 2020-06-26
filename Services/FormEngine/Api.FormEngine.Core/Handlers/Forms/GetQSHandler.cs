using Api.FormEngine.Core.Commands.Forms.QuestionSet;
using Api.FormEngine.Core.ViewModels.QuestionSets;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.StoredProcedureModel;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Get question set
    /// </summary>
    public class GetQSHandler : IRequestHandler<GetQuestionSetRequest, QuestionSetDetails>
    {
        private readonly FormsEngineContext frmEnginecontext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQSHandler"/> class
        /// </summary>
        /// <param name="frmEnginecontext">object of frmEnginecontext being passed using dependency injection</param>
        /// <param name="mapper">object of mapper being passed using dependency injection</param>
        public GetQSHandler(FormsEngineContext frmEnginecontext, IMapper mapper)
        {
            this.frmEnginecontext = frmEnginecontext;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<QuestionSetDetails> Handle(GetQuestionSetRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // looks for the question set of the respective id from the forms engine Db
            List<QSdbDetail> formDefintionQS = new List<QSdbDetail>();
            List<FieldConstraintDetail> filedConstraintsDetails = new List<FieldConstraintDetail>();
            const string Today = "today";

            var questionSetDefinitionsStoredProc = frmEnginecontext.Set<QuestionSetDetailSpModel>().FromSqlRaw("GetQuestionSetById @QuestionSetId = {0}", request.Id).ToList();

            var filedConstraintsResultsStoredProc = frmEnginecontext.Set<FieldConstraintDetailSpModel>().FromSqlRaw("GetQuestionSetFieldConstraintById @QuestionSetId = {0}", request.Id).ToList();

            var questionSetDefinitions =
                mapper.Map<List<QuestionSetDetailSpModel>, List<QSdbDetail>>(questionSetDefinitionsStoredProc);

            var filedConstraintsResults =
                mapper.Map<List<FieldConstraintDetailSpModel>, List<FieldConstraintDetail>>(filedConstraintsResultsStoredProc);

            formDefintionQS.AddRange(questionSetDefinitions);
            if (filedConstraintsResults.Any())
            {
                filedConstraintsDetails.AddRange(filedConstraintsResults);
            }

            if (formDefintionQS?.Count() == 0)
            {
                return null;
            }

            var sectionIds = formDefintionQS.OrderBy(x => x.SectionSequence).Select(x => x.SectionId).Distinct().ToList();
            var fieldIds = formDefintionQS.OrderBy(x => x.FieldSequence).Select(x => x.FieldId).Distinct().ToList();

            var qset = formDefintionQS.FirstOrDefault();
            QuestionSetDetails questionSetDetails = mapper.Map<QSdbDetail, QuestionSetDetails>(qset);
            var sectionList = new List<Section>();
            var initialValues = new Dictionary<string, object>();
            foreach (var sectionId in sectionIds)
            {
                var fieldsList = new List<Field>();
                var section = new Section();
                foreach (var fieldId in fieldIds)
                {
                    var qssection = formDefintionQS.FirstOrDefault(x => x.SectionId == sectionId);
                    section = mapper.Map<QSdbDetail, Section>(qssection);
                    var qsfieldList = formDefintionQS.Where(x => x.SectionId == sectionId && x.FieldId == fieldId).ToList();

                    if (qsfieldList != null && qsfieldList.Count > 0)
                    {
                        Field fields = mapper.Map<QSdbDetail, Field>(qsfieldList.FirstOrDefault());
                        string guideValue = string.Empty;
                        List<AnswerGuide> answerGuideList = mapper.Map<List<QSdbDetail>, List<AnswerGuide>>(qsfieldList);
                        var rules = new List<FieldRule>();
                        if (answerGuideList.Any(x => x.AnswerGuideId != Guid.Empty))
                        {
                            List<Option> options = mapper.Map<List<AnswerGuide>, List<Option>>(answerGuideList.Where(x => x.AnswerType.ToLower() == AnswerTypes.Value.ToString().ToLower())
                            .OrderBy(x => x.AnswerSequence).ToList());

                            fields.Options = options.Any() ? options : null;
                            fields.AnswerGuides = answerGuideList;

                            if (string.Equals(fields.Display, DisplayTypes.Required.ToString(), StringComparison.InvariantCultureIgnoreCase))
                            {
                                FieldRule rule = new FieldRule
                                {
                                    Name = fields.Display.ToLower(),
                                    Error = FieldErrorMessageConstants.FieldRequired
                                };
                                rules.Add(rule);
                            }

                            // Field Rules
                            var fieldRules = mapper.Map<List<AnswerGuide>, List<FieldRule>>(answerGuideList
                                 .Where(x => x.AnswerType.ToLower() != AnswerTypes.Value.ToString().ToLower())
                                 .ToList());
                            if (fieldRules.Any())
                            {
                                rules.AddRange(fieldRules);
                            }

                            var dateAnswerGuide = answerGuideList.Where(x => x.AnswerType.Equals(AnswerTypes.Date.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
                            if (dateAnswerGuide != null && dateAnswerGuide.Count > 0)
                            {
                                fields.PastDateAllowed = dateAnswerGuide.Any(x => x.Min == Today) ? "false" : null;
                                fields.MinDateField = dateAnswerGuide.Any(x => x.Min != Today) ? dateAnswerGuide.FirstOrDefault(x => x.Min != Today).Min : null;
                                fields.FutureDateAllowed = dateAnswerGuide.Any(x => x.Max == Today) ? "false" : null;
                                fields.MaxDateField = dateAnswerGuide.Any(x => x.Max != Today) ? dateAnswerGuide.FirstOrDefault(x => x.Max != Today).Max : null;
                            }
                            var defaultValue = answerGuideList.Where(x => x.IsDefault == "1").Select(x => x.AnswerGuideValue).ToArray();
                            guideValue = defaultValue.Count() > 1 ? "[" + string.Join(",", defaultValue) + "]" : defaultValue.FirstOrDefault();
                        }

                        fields.Rule = rules?.Count <= 0 ? null : rules;
                        if (fields.DisplayConstraint?.ToLower() == ConstraintTypes.Depends.ToString().ToLower() && filedConstraintsDetails.Count > 0)
                        {
                            var fieldConstraint = filedConstraintsDetails.Where(y => y.FieldId == fieldId).ToList();
                            if (fieldConstraint.Any())
                            {
                                var fieldcondition = mapper.Map<List<FieldConstraintDetail>, List<Condition>>(fieldConstraint);
                                Depends depends = new Depends
                                {
                                    Conditions = fieldcondition,
                                    ConditionsToPass = fields.ConstraintRule == "Any" ? fields.ConstraintRuleCount.ToString() : null,
                                };
                                fields.Depends = depends;
                            }
                        }
                        string fieldIdName = fields.FieldId.ToString() + "-" + fields.FieldNo.ToString();
                        fields.FieldName = fieldIdName;
                        fields.Id = fieldIdName;
                        fieldsList.Add(fields);
                        if (string.Equals(fields.FieldType, FieldTypes.CheckBox.ToString(), StringComparison.InvariantCultureIgnoreCase) && string.IsNullOrEmpty(guideValue))
                        {
                            initialValues.Add(fieldIdName, Array.Empty<string>());
                        }
                        else
                        {
                            initialValues.Add(fieldIdName, guideValue ?? string.Empty);
                        }
                    }
                }

                var sectionConstraint = filedConstraintsDetails.Where(y => y.SectionId == sectionId).ToList();
                if (sectionConstraint.Any())
                {
                    var fieldcondition = mapper.Map<List<FieldConstraintDetail>, List<Condition>>(sectionConstraint);
                    Depends depends = new Depends
                    {
                        Conditions = fieldcondition
                    };
                    section.Depends = depends;
                }
                section.Fields = fieldsList;
                sectionList.Add(section);
            }

            string initialValuesjson = JsonConvert.SerializeObject(initialValues, Formatting.Indented);

            QuestionSetDefinition questionSetDefinition = new QuestionSetDefinition
            {
                QSId = request.Id,
                Label = qset.Label,
                Helptext = qset.Helptext,
                Description = qset.Description,
                Sections = sectionList
            };
            var questionSetJson = JsonConvert.SerializeObject(questionSetDefinition, Formatting.Indented);

            int jsonlastindex = questionSetJson.LastIndexOf("}", StringComparison.OrdinalIgnoreCase);
            questionSetJson = questionSetJson.Remove(jsonlastindex);
            questionSetJson += ",\"initialValues\":" + initialValuesjson + "}";

            questionSetDetails.Definition = questionSetJson;

            frmEnginecontext.Database.CloseConnection();

            return questionSetDetails;
        }
    }
}