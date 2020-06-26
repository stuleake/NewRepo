using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.Services.Parsers.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TQ.Core.Repository;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Services.Parsers
{
    /// <inheritdoc/>
    public class TaxonomyParser : IParser<TaxonomyParserModel, IEnumerable<Taxonomy>>
    {
        private readonly IReadOnlyRepository<QS> questionSetRepository;
        private readonly IReadOnlyRepository<Taxonomy> taxonomyRepository;

        /// <inheritdoc/>
        public TaxonomyParser(
            IReadOnlyRepository<QS> questionSetRepository,
            IReadOnlyRepository<Taxonomy> taxonomyRepository)
        {
            this.questionSetRepository = questionSetRepository;
            this.taxonomyRepository = taxonomyRepository;
        }

        /// <inheritdoc/>
        public ParseResult<IEnumerable<Taxonomy>> Parse(TaxonomyParserModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var addedTaxonomies = new List<Taxonomy>();
            var updatedTaxonomies = new List<Taxonomy>();

            foreach (var question in model.QuestionSets)
            {
                // Convert taxonomies into JSON and store it into database
                string taxonomyJson = JsonConvert.SerializeObject(model.Taxonomies);
                var questionNumber = Convert.ToInt32(question.QsNo);

                var questionSetExists = questionSetRepository.GetQueryable()
                    .Where(qs => qs.QSNo == questionNumber)
                    .OrderByDescending(qs => qs.QSVersion)
                    .FirstOrDefault();

                var isStatusActive = questionSetExists != null && questionSetExists.StatusId == FormStructureConstants.ActiveQsStatusNumber;

                Taxonomy taxonomy = new Taxonomy
                {
                    QsNo = questionNumber,
                    Tenant = question.Tenant,
                    LanguageCode = question.Language,
                    TaxonomyDictionary = taxonomyJson
                };

                // to update if the status is draft and create new if status is active
                if (questionSetExists == null || isStatusActive)
                {
                    string questionSetVersion = isStatusActive
                        ? Convert.ToString(questionSetExists.QSVersion + FormStructureConstants.DraftQsStatusNumber)
                        : FormStructureConstants.DefaultVersion.ToString();

                    taxonomy.QsVersion = questionSetVersion;
                    updatedTaxonomies.Add(taxonomy);
                }
                else
                {
                    // Check if taxonomy already exists
                    var taxonomyExists = taxonomyRepository.GetQueryable()
                        .FirstOrDefault(t => t.QsNo == questionNumber
                            && t.Tenant == question.Tenant
                            && t.LanguageCode == question.Language
                            && t.QsVersion == Convert.ToString(questionSetExists.QSVersion));

                    if (taxonomyExists == null)
                    {
                        taxonomy.QsVersion = Convert.ToString(questionSetExists.QSVersion);
                        addedTaxonomies.Add(taxonomy);
                    }
                    else
                    {
                        taxonomyExists.TaxonomyDictionary = taxonomyJson;
                        updatedTaxonomies.Add(taxonomyExists);
                    }
                }
            }

            return new ParseResult<IEnumerable<Taxonomy>>
            {
                Added = addedTaxonomies,
                Updated = updatedTaxonomies
            };
        }
    }
}