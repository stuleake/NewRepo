using Api.FeeCalculator.Core.Commands.FeeCalculator;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Data.FeeCalculator;
using TQ.Data.FeeCalculator.Schemas.Dbo;

namespace Api.FeeCalculator.Core.Handlers.FeeCalculator
{
    /// <summary>
    /// Handler class to import and save the Rules to DB.
    /// </summary>
    public class ImportFeeCalcRulesHandler : IRequestHandler<FeeCalculatorRules, bool>
    {
        private readonly IMapper mapper;
        private readonly FeeCalculatorContext feeCalculatorContext;
        private const string Fee = "fee";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFeeCalcRulesHandler"/> class.
        /// </summary>
        /// <param name="feeCalculatorContext">Fee Calculator DB context for country specified in request header</param>
        /// <param name="mapper">object of mapper being passed using dependency injection</param>
        public ImportFeeCalcRulesHandler(FeeCalculatorContext feeCalculatorContext, IMapper mapper)
        {
            this.mapper = mapper;
            this.feeCalculatorContext = feeCalculatorContext;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(FeeCalculatorRules request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IDbContextTransaction transaction = feeCalculatorContext.Database.BeginTransaction();

            try
            {
                var defPackage = new DefPackage
                {
                    FileName = request.FileName,
                    CreatedDate = DateTime.Now,
                    CreatedBy = request.UserId,
                    Status = RuleDefPackageStatusConstants.Draft,
                };
                feeCalculatorContext.DefPackages.Add(defPackage);
                await feeCalculatorContext.SaveChangesAsync().ConfigureAwait(false);
                var categories = feeCalculatorContext.Categories.ToList();
                var paramDatatypes = feeCalculatorContext.ParamDataTypes.ToList();
                await InsertMasterParametersAsync(request.MasterParameters, paramDatatypes, request.Country).ConfigureAwait(false);
                foreach (var rule in request.Rules)
                {
                    var ruleCategory = categories.FirstOrDefault(x => x.CategoryName.Equals(rule.Category, StringComparison.OrdinalIgnoreCase));

                    var ruleDef = mapper.Map<RuleDef>(rule);
                    ruleDef.DefPackageId = defPackage.DefPackageId;
                    ruleDef.CategoryId = ruleCategory.CategoryId;
                    ruleDef.Tenant = request.Country;
                    ruleDef.Product = Fee;
                    ruleDef.CreatedDate = DateTime.UtcNow;
                    ruleDef.CreatedBy = request.UserId;

                    feeCalculatorContext.RuleDefs.Add(ruleDef);
                    int sequence = 0;
                    foreach (var param in rule.Parameters)
                    {
                        var currentParam = feeCalculatorContext.Parameters.FirstOrDefault(x => x.Name.ToLower() == param.Name.ToLower() &&
                        x.Tenant.ToLower() == request.Country.ToLower()
                        && x.Product.ToLower() == Fee);
                        if (currentParam == null)
                        {
                            var datatype = paramDatatypes.FirstOrDefault(x => x.ParamDataTypeName.Equals(param.Datatype, StringComparison.OrdinalIgnoreCase));

                            if (datatype == null)
                            {
                                throw new TQException($"Unsupported Data type Encountered in Rule No: {rule.RuleId} for parameter : {param.Name}");
                            }
                            currentParam = new Parameter
                            {
                                Name = param.Name,
                                ParamDataTypeId = datatype.ParamDataTypeId,
                                Tenant = request.Country,
                                Product = Fee
                            };
                            feeCalculatorContext.Parameters.Add(currentParam);
                        }

                        if (!Enum.TryParse(param.ParameterType, true, out ParameterTypes paramtype))
                        {
                            throw new TQException($"Invalid parameter type. for Rule No {rule.RuleId}");
                        }

                        feeCalculatorContext.RuleDefParameterMappings.Add(new RuleDefParameterMapping
                        {
                            RuleDefId = ruleDef.RuleDefId,
                            ParameterId = currentParam.ParameterId,
                            Sequence = sequence++,
                            ParameterType = paramtype.ToString(),
                            OutputOperation = param.OutputOperation
                        });
                    }
                }
                await feeCalculatorContext.SaveChangesAsync().ConfigureAwait(false);
                await transaction.CommitAsync().ConfigureAwait(false);
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
            return true;
        }

        private async Task InsertMasterParametersAsync(IDictionary<string, string> parameters, List<ParamDataType> dataTypes, string tenant)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    if (float.TryParse(param.Value, out _))
                    {
                        AddMasterParameter(dataTypes, "number", param.Key, param.Value, tenant);
                    }
                    else if (bool.TryParse(param.Value, out _))
                    {
                        AddMasterParameter(dataTypes, "bool", param.Key, param.Value, tenant);
                    }
                    else
                    {
                        AddMasterParameter(dataTypes, "string", param.Key, param.Value, tenant);
                    }
                    await feeCalculatorContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }
        }

        private void AddMasterParameter(List<ParamDataType> paramDataTypes, string dataTypeName, string name, string value, string tenant)
        {
            var existingParam = feeCalculatorContext.Parameters.FirstOrDefault(x => x.Name.ToLower() == name.ToLower() && x.Tenant.ToLower() == tenant.ToLower()
            && x.Product.ToLower() == Fee.ToLower());
            var currentDatatype = paramDataTypes.FirstOrDefault(x => x.ParamDataTypeName.Equals(dataTypeName, StringComparison.OrdinalIgnoreCase));
            if (currentDatatype == null)
            {
                throw new TQException($"Missing datatype {dataTypeName}");
            }
            if (existingParam != null)
            {
                existingParam.MasterValue = value;
                existingParam.ParamDataTypeId = currentDatatype.ParamDataTypeId;
            }
            else
            {
                feeCalculatorContext.Parameters.Add(new Parameter
                {
                    Product = Fee,
                    Tenant = tenant,
                    Name = name,
                    MasterValue = value,
                    ParamDataTypeId = currentDatatype.ParamDataTypeId
                });
            }
        }
    }
}