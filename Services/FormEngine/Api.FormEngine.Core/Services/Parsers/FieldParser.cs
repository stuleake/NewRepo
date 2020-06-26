using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.Services.Parsers.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class FieldParser : IParser<FieldParserModel, IEnumerable<Field>>
    {
        private readonly IReadOnlyRepository<Rule> ruleRepository;
        private readonly IReadOnlyRepository<Constraint> constraintRepository;
        private readonly IReadOnlyRepository<QS> questionSetRepository;
        private readonly IMapper mapper;

        /// <inheritdoc/>
        public FieldParser(
            IReadOnlyRepository<Rule> ruleRepository,
            IReadOnlyRepository<Constraint> constraintRepository,
            IReadOnlyRepository<QS> questionSetRepository,
            IMapper mapper)
        {
            this.ruleRepository = ruleRepository;
            this.constraintRepository = constraintRepository;
            this.questionSetRepository = questionSetRepository;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<Field>> Parse(FieldParserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var questionSet = questionSetRepository.GetQueryable().FirstOrDefault(qs => qs.QSNo == Convert.ToInt32(model.QuestionSet.QsNo));
            var dateTimeNow = DateTime.UtcNow;
            var fields = new List<Field>();

            foreach (var section in model.QuestionSet.Sections)
            {
                foreach (var field in section.Fields)
                {
                    if (field.Action == ApplicationConstants.Delete)
                    {
                        continue;
                    }

                    var fieldDB = mapper.Map<ViewModels.SheetModels.Field, Field>(field);

                    fieldDB.FieldVersion = questionSet.QSVersion;
                    fieldDB.CreatedDate = dateTimeNow;
                    fieldDB.LastModifiedDate = dateTimeNow;
                    fieldDB.CreatedBy = model.UserId;
                    fieldDB.LastModifiedBy = model.UserId;

                    var fieldListInDependencies = model.Dependencies.Where(d => d.FieldNo == field.FieldNo).ToList();
                    var ruleIdAll = ruleRepository.GetQueryable()
                        .Where(rules => rules.Rules == FormStructureConstants.All)
                        .Select(rules => rules.RuleId)
                        .FirstOrDefault();

                    fieldDB.AnswerRuleId = ruleIdAll;

                    if (fieldListInDependencies.Any())
                    {
                        var constraintsId = constraintRepository.GetQueryable()
                            .Where(c => c.Constraints == FormStructureConstants.Depends)
                            .Select(s => s.ConstraintId)
                            .FirstOrDefault();

                        fieldDB.DisplayConstraintId = constraintsId;

                        if (fieldListInDependencies.Count.ToString() == fieldListInDependencies
                            .Select(d => d.DependsCount)
                            .FirstOrDefault())
                        {
                            fieldDB.ConstraintRuleId = ruleIdAll;
                        }
                        else
                        {
                            var ruleId = ruleRepository.GetQueryable()
                                .Where(r => r.Rules == FormStructureConstants.Any)
                                .Select(r => r.RuleId)
                                .FirstOrDefault();

                            fieldDB.ConstraintRuleId = ruleId;
                            fieldDB.ConstraintRuleCount = Convert.ToInt32(fieldListInDependencies.Select(d => d.DependsCount).FirstOrDefault());
                        }
                    }
                    else
                    {
                        var constraintsId = constraintRepository.GetQueryable()
                            .Where(c => c.Constraints == FormStructureConstants.SoloString)
                            .Select(s => s.ConstraintId)
                            .FirstOrDefault();
                        fieldDB.DisplayConstraintId = constraintsId;
                    }

                    fields.Add(fieldDB);
                }
            }

            return new ParseResult<IEnumerable<Field>>
            {
                Added = fields
            };
        }
    }
}