using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.ViewModels;
using CsvHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Forms;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to upload taxonomy csv
    /// </summary>
    public class UploadTaxonomyHandler : IRequestHandler<UploadTaxonomy, TaxonomyUploadResponse>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UploadTaxonomyHandler"/> class.
        /// </summary>
        /// <param name="formsEngineContext">object of frmEnginecontext being passed using dependency injection</param>
        public UploadTaxonomyHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public async Task<TaxonomyUploadResponse> Handle(UploadTaxonomy request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var taxonomyUploadResponse = new TaxonomyUploadResponse();
            var fileStream = request.FileContent.OpenReadStream();
            if (request.FileContent.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            {
                var responseMessage = new StringBuilder();
                int noOfRecordsAdded = 0;
                int noOfRecordsUpdated = 0;
                var readTaxonomies = ReadTaxonomies(fileStream);
                var mappedTaxonomies = MapTaxonomies(readTaxonomies);
                foreach (var taxonomy in mappedTaxonomies)
                {
                    // Search for question set with same QsNo and version as taxonomy
                    var questionSetExists = formsEngineContext.QS.FirstOrDefault(question => question.QSNo == taxonomy.QsNo && question.QSVersion == Convert.ToDecimal(taxonomy.QsVersion));

                    // If Question set doesn't exist don't add the taxonomy into the database
                    if (questionSetExists != null)
                    {
                        // If question set status is not draft, don't add the taxonomy into the database
                        var isStatusDraft = questionSetExists.StatusId == FormStructureConstants.DraftQsStatusNumber;
                        if (isStatusDraft)
                        {
                            // Check if taxonomy already exists in db
                            var taxonomyExists = await formsEngineContext.Taxonomy.FirstOrDefaultAsync(taxo => taxo.QsNo == taxonomy.QsNo
                            && taxo.QsVersion == taxonomy.QsVersion
                            && taxo.Tenant == taxonomy.Tenant
                            && taxo.LanguageCode == taxonomy.LanguageCode).ConfigureAwait(false);

                            // If taxonomy doesn't exist in the database, add the new taxonomy, else update the existing one
                            if (taxonomyExists == null)
                            {
                                formsEngineContext.Taxonomy.Add(taxonomy);
                                noOfRecordsAdded += 1;
                            }
                            else
                            {
                                taxonomyExists.TaxonomyDictionary = taxonomy.TaxonomyDictionary;
                                noOfRecordsUpdated += 1;
                            }
                        }
                        else
                        {
                            responseMessage.AppendLine($"Version {questionSetExists.QSVersion} of Question set {questionSetExists.QSNo} is not a draft, hence not added");
                        }
                    }
                    else
                    {
                        responseMessage.AppendLine($"Version {taxonomy.QsVersion} of Question set {taxonomy.QsNo} does not exist, hence not added");
                    }
                }
                formsEngineContext.SaveChanges();
                responseMessage.AppendLine($"{noOfRecordsAdded} Records added, {noOfRecordsUpdated} Records updated");
                taxonomyUploadResponse.Response = responseMessage.ToString();
            }
            else
            {
                throw new TQException("File is not a csv");
            }
            return taxonomyUploadResponse;
        }

        /// <summary>
        /// Read taxonomies from the csv
        /// </summary>
        /// <param name="fileContent">Stream containing the file content</param>
        /// <returns>IEnumerable of TaxonomyDto</returns>
        public IEnumerable<TaxonomyDto> ReadTaxonomies(Stream fileContent)
        {
            var taxonomies = new List<TaxonomyDto>();
            using (var csv = new CsvParser(new StreamReader(fileContent), CultureInfo.InvariantCulture))
            {
                // storing header values
                var headers = csv.Read().ToList();

                // finding the QsNo
                int questionSetNoIndex = headers.FindIndex(str => str.Contains("QsNo", StringComparison.OrdinalIgnoreCase));

                // finding the QsVersion
                int questionSetVersionIndex = headers.FindIndex(str => str.Contains("QsVersion", StringComparison.OrdinalIgnoreCase));
                int keyIndex = headers.FindIndex(str => str.Contains("Key", StringComparison.OrdinalIgnoreCase));
                var check = true;
                while (check)
                {
                    var columns = csv.Read();
                    if (columns != null)
                    {
                        if (columns.Any(c => string.IsNullOrWhiteSpace(c)))
                        {
                            continue;
                        }
                        var questionSetNo = Convert.ToInt32(columns[questionSetNoIndex]);
                        var questionSetVersion = columns[questionSetVersionIndex];

                        // adding taxonomy for each language in dictionary
                        const int defaultHeaderCount = 3;
                        for (int i = defaultHeaderCount; i < headers.Count; i++)
                        {
                            if (string.IsNullOrWhiteSpace(headers[i]))
                            {
                                continue;
                            }

                            var taxonomyExists = taxonomies.FirstOrDefault(taxo => taxo.QsNo == questionSetNo
                            && taxo.QsVersion == questionSetVersion
                            && taxo.LanguageTenant == headers[i]);

                            if (taxonomyExists == null)
                            {
                                var taxonomy = new TaxonomyDto
                                {
                                    QsNo = questionSetNo,
                                    QsVersion = questionSetVersion,
                                    LanguageTenant = headers[i],
                                };
                                taxonomy.TaxonomyValues.Add(columns[keyIndex], columns[i]);
                                taxonomies.Add(taxonomy);
                            }
                            else
                            {
                                taxonomyExists.TaxonomyValues.Add(columns[keyIndex], columns[i]);
                            }
                        }
                    }
                    else
                    {
                        check = false;
                    }
                }
            }
            return taxonomies;
        }

        /// <summary>
        /// Map the taxonomy dto into taxonomy objects
        /// </summary>
        /// <param name="taxonomyViews">IEnumerable of taxonomyViews</param>
        /// <returns>IEnumerable of Taxonomy objects</returns>
        private IEnumerable<Taxonomy> MapTaxonomies(IEnumerable<TaxonomyDto> taxonomyViews)
        {
            var mappedTaxonomies = new List<Taxonomy>();
            foreach (var taxo in taxonomyViews)
            {
                var languageTenant = taxo.LanguageTenant.Split('-');
                var taxonomy = new Taxonomy
                {
                    QsNo = taxo.QsNo,
                    QsVersion = taxo.QsVersion,
                    LanguageCode = languageTenant[0],
                    Tenant = languageTenant[1],
                    TaxonomyDictionary = JsonConvert.SerializeObject(taxo.TaxonomyValues, Formatting.Indented)
                };
                mappedTaxonomies.Add(taxonomy);
            }
            return mappedTaxonomies;
        }
    }
}