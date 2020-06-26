using Api.FeeCalculator.Core.Commands.FeeCalculator;
using Api.FeeCalculator.Core.Helpers.JSEngine;
using Api.FeeCalculator.Core.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Data.FeeCalculator;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace Api.FeeCalculator.Core.Handlers.FeeCalculator
{
    /// <summary>
    /// Handler class to calculate fee for input values
    /// </summary>
    public class FeeCalculatorHandler : IRequestHandler<FeeCalculatorData, FeeCalculatorResponseModel>
    {
        private readonly FeeCalculatorContext context;
        private readonly JsEngine engine;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeCalculatorHandler"/> class.
        /// </summary>
        /// <param name="context">Fee calculator DB context for country specified in request header</param>
        /// <param name="engine">JsEngine object to execute javascript in sandbox environment</param>
        /// <param name="mapper">object of model mapper</param>
        public FeeCalculatorHandler(FeeCalculatorContext context, JsEngine engine, IMapper mapper)
        {
            this.context = context;
            this.engine = engine;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<FeeCalculatorResponseModel> Handle(FeeCalculatorData request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IDbContextTransaction transaction = context.Database.BeginTransaction();
            try
            {
                var rules = (await (from ruledef in context.RuleDefs
                                    join ruleDefParameterMapping in context.RuleDefParameterMappings on ruledef.RuleDefId equals ruleDefParameterMapping.RuleDefId
                                    join parameter in context.Parameters on ruleDefParameterMapping.ParameterId equals parameter.ParameterId
                                    join parameterDataType in context.ParamDataTypes on parameter.ParamDataTypeId equals parameterDataType.ParamDataTypeId
                                    where ruledef.StartDate <= DateTime.UtcNow && (ruledef.EndDate == null || ruledef.EndDate >= DateTime.UtcNow)
                                    && ruledef.Tenant.ToLower() == request.Country.ToLower() && ruledef.Product.ToLower() == "fee"
                                    select new RuleDetailsModel
                                    {
                                        RuleDefId = ruledef.RuleDefId,
                                        RuleName = ruledef.RuleName,
                                        RuleDef = ruledef.RuleDefinition,
                                        RuleReferenceId = ruledef.ReferenceId,
                                        ParameterId = parameter.ParameterId,
                                        ParameterName = parameter.Name,
                                        DataTypeId = parameterDataType.ParamDataTypeId,
                                        DataType = parameterDataType.ParamDataTypeName,
                                        Sequence = ruleDefParameterMapping.Sequence,
                                        CategoryId = ruledef.CategoryId,
                                        ParamType = ruleDefParameterMapping.ParameterType,
                                        OutputOperation = ruleDefParameterMapping.OutputOperation,
                                    }).ToListAsync().ConfigureAwait(false)).GroupBy(x => x.RuleDefId)
                                  .ToDictionary(ans => ans.Key, ans => ans.ToList());

                await RemoveExistingAnswersAsync(context, SessionTypeConstants.QSR, request.SessionId).ConfigureAwait(false);
                var qsr = new Qsr
                {
                    QsrId = request.SessionId,
                    QSCollectionId = request.QsCollectionId.Value,
                    CreateDate = DateTime.UtcNow,
                    CreatedBy = request.UserId
                };
                var masterParams = context.Parameters.Where(param => !string.IsNullOrEmpty(param.MasterValue) &&
                                                                     param.Tenant.ToLower() == request.Country.ToLower() &&
                                                                     param.Product.ToLower() == "fee").ToList();
                context.Qsrs.Add(qsr);
                await context.SaveChangesAsync().ConfigureAwait(false);
                var calculationSteps = new List<Step>();
                var answers = new List<Answer>();
                foreach (var value in request.Answers)
                {
                    answers.Add(new Answer
                    {
                        SessionId = qsr.QsrId,
                        SessionType = SessionTypeConstants.QSR,
                        ParameterName = value.ParameterName,
                        ParameterAnswer = value.Answer
                    });
                }

                var applicableRules = rules.Where(x => !calculationSteps.Any(r => r.RuleId != x.Key) &&
                    x.Value.Where(prmType => prmType.ParamType.Equals(ParameterTypes.In.ToString(), StringComparison.OrdinalIgnoreCase) &&
                    !prmType.ParameterName.Equals("Language", StringComparison.OrdinalIgnoreCase)).All(rule => answers.Any(ans =>
                    ans.ParameterName.Equals(rule.ParameterName, StringComparison.OrdinalIgnoreCase)) || masterParams.Any(param =>
                    param.Name.Equals(rule.ParameterName, StringComparison.OrdinalIgnoreCase)))).Select(y => y).ToDictionary(x => x.Key, x => x.Value);

                while (applicableRules != null && applicableRules.Count > 0)
                {
                    foreach (var rule in applicableRules.Where(rule => rule.Value != null && rule.Value.Count > 0))
                    {
                        var inputs = new StringBuilder();
                        var result = ExecuteFunction(rule.Value, answers, masterParams, ref inputs, request.Language);

                        if (result != null)
                        {
                            var outputData = rule.Value.FirstOrDefault(x => x.ParamType.Equals(ParameterTypes.Out.ToString(), StringComparison.OrdinalIgnoreCase));
                            result.ParameterName = outputData?.ParameterName;
                            result.Datatype = outputData?.DataType;
                            result.IsFinalOutput = outputData != null ? outputData.IsFinalOutput : default;
                            result.RuleNo = rule.Value[0].RuleReferenceId;
                            result.OutPutOperation = outputData?.OutputOperation;

                            var existingResult = calculationSteps.FirstOrDefault(outParam => outParam.OutputParamName.Equals(result.ParameterName, StringComparison.OrdinalIgnoreCase) &&
                            outParam.OutputDataType.Equals(result.Datatype, StringComparison.OrdinalIgnoreCase));

                            if (existingResult != null && result.OutPutOperation.Equals(OutputOperationContstants.Add, StringComparison.OrdinalIgnoreCase) && result.Datatype == "number")
                            {
                                result.Value = (Convert.ToInt32(result.Value) + Convert.ToInt32(existingResult.Output)).ToString();
                            }

                            calculationSteps.Add(new Step
                            {
                                Description = result.Description,
                                Inputs = inputs.ToString(),
                                Output = result.Value,
                                RuleNo = rule.Value[0].RuleReferenceId,
                                RuleId = rule.Key,
                                CategoryId = rule.Value[0].CategoryId,
                                SessionId = qsr.QsrId,
                                SessionType = SessionTypeConstants.QSR,
                                OutputParamName = result.ParameterName,
                                OutputDataType = result.Datatype,
                                IsFinalOutput = result.IsFinalOutput
                            });

                            answers.Add(new Answer { SessionId = qsr.QsrId, SessionType = SessionTypeConstants.QSR, ParameterName = result.ParameterName, ParameterAnswer = result.Value });
                        }
                        else
                        {
                            calculationSteps.Add(new Step
                            {
                                Inputs = inputs.ToString(),
                                RuleNo = rule.Value[0].RuleReferenceId,
                                RuleId = rule.Key,
                                CategoryId = rule.Value[0].CategoryId,
                                SessionId = qsr.QsrId,
                                SessionType = SessionTypeConstants.QSR
                            });
                        }
                    }

                    applicableRules = rules.Where(rule => calculationSteps.All(calcStep => calcStep.RuleId != rule.Key) && rule.Value.Where(rl =>
                    rl.ParamType.Equals(ParameterTypes.In.ToString(), StringComparison.OrdinalIgnoreCase) && !rl.ParameterName.Equals("Language", StringComparison.OrdinalIgnoreCase))
                    .All(rule => answers.Any(ans => ans.ParameterName.Equals(rule.ParameterName, StringComparison.OrdinalIgnoreCase)) || masterParams.Any(param =>
                     param.Name.Equals(rule.ParameterName, StringComparison.OrdinalIgnoreCase)))).Select(ruleDeatail => ruleDeatail).ToDictionary(ruledeatil => ruledeatil.Key, x => x.Value);
                }

                context.Answers.AddRange(answers);
                var response = new FeeCalculatorResponseModel
                {
                    SessionType = request.SessionType,
                    SessionId = request.SessionId
                };
                if (calculationSteps != null && calculationSteps.Count > 0)
                {
                    context.Steps.AddRange(calculationSteps.Where(step => !string.IsNullOrEmpty(step.Output)));
                    response.CalculationSteps = mapper.Map<List<CalculationStepModel>>(calculationSteps.Where(step => !string.IsNullOrEmpty(step.Output)));
                }
                await context.SaveChangesAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);

                return response;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }

        private JsResponseModel ExecuteFunction(List<RuleDetailsModel> rule, List<Answer> answers, List<Parameter> masterParams, ref StringBuilder inputs, string language)
        {
            var funcDefinition = new StringBuilder();
            var funcExecution = new StringBuilder();
            funcDefinition.Append($"function Fee_{rule.FirstOrDefault()?.RuleReferenceId}(");
            funcExecution.Append($"Fee_{rule.FirstOrDefault()?.RuleReferenceId}(");
            foreach (var param in rule.Where(x => x.ParamType.Equals("in", StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Sequence))
            {
                funcDefinition.Append(param.ParameterName + ",");
                var value = answers.LastOrDefault(x => x.ParameterName.Equals(param.ParameterName, StringComparison.OrdinalIgnoreCase))?.ParameterAnswer;
                value ??= masterParams.FirstOrDefault(x => x.Name.Equals(param.ParameterName, StringComparison.OrdinalIgnoreCase))?.MasterValue ?? "null";
                if (param.ParameterName.Equals("language", StringComparison.OrdinalIgnoreCase))
                {
                    value = language;
                }
                switch (param.DataType)
                {
                    case "bool":
                        funcExecution.Append(value.ToLower() + ",");
                        inputs.Append(value.ToLower() + ",");
                        break;

                    case "number":
                        funcExecution.Append(Convert.ToDouble(value) + ",");
                        inputs.Append(value.ToLower() + ",");
                        break;

                    case "string":
                        funcExecution.Append("\"" + value + "\",");
                        inputs.Append(value.ToLower() + ",");
                        break;

                    default:
                        funcExecution.Append("null");
                        inputs.Append(value.ToLower() + ",");
                        break;
                }
            }
            funcDefinition.Length--;
            funcExecution.Length--;
            inputs.Length--;
            funcDefinition.Append(") {" + rule.FirstOrDefault()?.RuleDef + " }");
            funcExecution.Append(");");

            var result = engine.ExecuteJavascript(funcDefinition.ToString(), funcExecution.ToString());
            return result;
        }

        private static async Task RemoveExistingAnswersAsync(FeeCalculatorContext context, string sessionType, Guid sessionId)
        {
            if (sessionType.Equals(SessionTypeConstants.QSR, StringComparison.OrdinalIgnoreCase))
            {
                var qsr = await context.Qsrs.FirstOrDefaultAsync(qsr => qsr.QsrId.Equals(sessionId)).ConfigureAwait(false);

                if (qsr == null)
                {
                    return;
                }

                context.Qsrs.Remove(qsr);
                context.Answers.RemoveRange(context.Answers.Where(ans => ans.SessionType.ToLower() == sessionType.ToLower()
                 && ans.SessionId.Equals(sessionId)));

                var steps = context.Steps.Where(step => step.SessionType.ToLower() == sessionType.ToLower() && step.SessionId.Equals(sessionId)).ToList();
                if (steps != null && steps.Any())
                {
                    context.Steps.RemoveRange(steps);
                }
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}