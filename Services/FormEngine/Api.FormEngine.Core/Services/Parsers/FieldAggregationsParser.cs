using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.ViewModels.SheetModels;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class FieldAggregationsParser : IParser<IEnumerable<Aggregations>, IEnumerable<FieldAggregation>>
    {
        private readonly IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.Field> fieldRepository;
        private readonly IReadOnlyRepository<Function> functionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldAggregationsParser"/> class.
        /// </summary>
        /// <param name="fieldRepository">Field repository to query.</param>
        /// <param name="functionRepository">Function repository to query.</param>
        public FieldAggregationsParser(
            IReadOnlyRepository<TQ.Data.FormEngine.Schemas.Forms.Field> fieldRepository,
            IReadOnlyRepository<Function> functionRepository)
        {
            this.fieldRepository = fieldRepository;
            this.functionRepository = functionRepository;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<FieldAggregation>> Parse(IEnumerable<Aggregations> model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var fieldAggregations = new List<FieldAggregation>();

            foreach (var aggregation in model)
            {
                if (!fieldRepository.GetLocalQueryable()
                    .Any(x => x.FieldNo == Convert.ToInt32(aggregation.FieldNo)))
                {
                    continue;
                }

                var fieldId = fieldRepository.GetLocalQueryable()
                    .Where(f => f.FieldNo == Convert.ToInt32(aggregation.FieldNo))
                    .Select(f => f.FieldId)
                    .FirstOrDefault();

                var fieldAggregationId = fieldRepository.GetLocalQueryable()
                    .Where(f => f.FieldNo == Convert.ToInt32(aggregation.AggregatedFieldNo))
                    .Select(f => f.FieldId)
                    .FirstOrDefault();

                var functionId = functionRepository.GetQueryable()
                    .Where(function => function.Functions == aggregation.Function)
                    .Select(function => function.FunctionsId)
                    .FirstOrDefault();

                fieldAggregations.Add(new FieldAggregation
                {
                    FieldId = fieldId,
                    FieldAggregationId = fieldAggregationId,
                    FunctionId = functionId,
                    Sequence = Convert.ToInt32(aggregation.Priority)
                });
            }

            return new ParseResult<IEnumerable<FieldAggregation>>
            {
                Added = fieldAggregations
            };
        }
    }
}