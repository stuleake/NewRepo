using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.Handlers.Forms.Interfaces;
using Api.FormEngine.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    /// Class for the TaxonomyCsvDownload handler
    /// </summary>
    public class DownloadTaxonomyCsvHandler : IDownloadTaxonomyCsvHandler
    {
        private readonly FormsEngineContext formsEngineContext;

        private readonly List<string> headerList = new List<string>
                                                    {
                                                        ApplicationConstants.QsNumberHeader,
                                                        ApplicationConstants.QsVersionHeader,
                                                        ApplicationConstants.KeyHeader
                                                    };

        private const int HeadersLength = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadTaxonomyCsvHandler"/> class.
        /// </summary>
        /// <param name="formsEngineContext">Object of forms-engine context</param>
        public DownloadTaxonomyCsvHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <summary>
        /// Handler for taxonomy download
        /// </summary>
        /// <param name="request">Request object </param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns comma separated string</returns>
        public async Task<TaxonomyDownload> Handle(DownloadTaxonomyCsv request, CancellationToken cancellationToken)
        {
            return await DownloadTaxonomyCsvAsync(request, formsEngineContext).ConfigureAwait(false);
        }

        /// <summary>
        /// Method to create csv from db data
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="formsEngineContext">Forms engine-context</param>
        /// <returns>Returns csv string </returns>
        public async Task<TaxonomyDownload> DownloadTaxonomyCsvAsync(DownloadTaxonomyCsv request, FormsEngineContext formsEngineContext)
        {
            if (formsEngineContext == null)
            {
                throw new ArgumentNullException(nameof(formsEngineContext));
            }
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            var csv = string.Empty;
            if (request.QsNo != null && request.QsVersion != null && !string.IsNullOrEmpty(request.QsNo) && !string.IsNullOrEmpty(request.QsVersion))
            {
                // Get particular record from db and send csv string
                var res = await formsEngineContext.QS.Where(x => x.QSNo == Convert.ToInt32(request.QsNo) && x.QSVersion == Convert.ToDecimal(request.QsVersion)).ToListAsync().ConfigureAwait(false);
                if (res.Any())
                {
                    var groupedQs = await formsEngineContext.Taxonomy
                                                       .Where(x => x.QsNo == Convert.ToInt32(request.QsNo) &&
                                                                   x.QsVersion == request.QsVersion)
                                                       .ToListAsync()
                                                       .ConfigureAwait(false);

                    var csvWithoutHeader = new StringBuilder();
                    csvWithoutHeader.AppendLine(GetCsvStringForQsVersion(groupedQs));
                    var header = string.Join(",", headerList) + Environment.NewLine;
                    csv = header + csvWithoutHeader;
                }
                else
                {
                    throw new TQException("QsNo and QsVersion not found in database!");
                }
            }
            else
            {
                // Get all records from db and send csv string
                var csvWithoutHeader = new StringBuilder();
                var questionSets = await formsEngineContext.QS.ToListAsync().ConfigureAwait(false);
                foreach (var qs in questionSets)
                {
                    var taxonomyQS = await formsEngineContext.Taxonomy.Where(x => x.QsNo == qs.QSNo && x.QsVersion == qs.QSVersion.ToString())
                                                                .ToListAsync()
                                                                .ConfigureAwait(false);
                    if (!taxonomyQS.Any())
                    {
                        continue;
                    }
                    csvWithoutHeader.AppendLine(GetCsvStringForQsVersion(taxonomyQS));
                }
                var header = string.Join(",", headerList) + Environment.NewLine;
                csv = header + csvWithoutHeader;
            }
            return new TaxonomyDownload
            {
                CsvString = csv
            };
        }

        private string GetCsvStringForQsVersion(List<Taxonomy> questionSets)
        {
            var csvWithoutHeader = new StringBuilder();
            var langCodeIdxList = new List<int>();
            var taxonomyDictList = new List<Dictionary<string, string>>();
            var qsno = string.Empty;
            var qsv = string.Empty;
            foreach (var item in questionSets)
            {
                // create L_C here from tenant/country and languageCode
                // deserialize dictionary here
                var deserializedTaxonomyDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.TaxonomyDictionary);
                var langCode = $"{item.LanguageCode}-{item.Tenant}";

                if (string.IsNullOrEmpty(qsno) || string.IsNullOrWhiteSpace(qsno))
                {
                    qsno = item.QsNo.ToString();
                }
                if (string.IsNullOrEmpty(qsv) || string.IsNullOrWhiteSpace(qsv))
                {
                    qsv = item.QsVersion;
                }
                langCodeIdxList.Add(GetLanguageCodeIndex(langCode));
                taxonomyDictList.Add(deserializedTaxonomyDict);
            }
            var combinedDict = taxonomyDictList.SelectMany(d => d)
                      .GroupBy(
                        kvp => kvp.Key,
                        (key, kvps) => new
                        {
                            Key = key,
                            Value =
                        string.Join(";", kvps.Select(x => $"\"{x.Value}\"").ToList())
                        })
                      .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var qs = string.Join(
                Environment.NewLine,
                combinedDict.Select(kvp => $"{qsno},{qsv},{kvp.Key}," + GetStringForValues(kvp.Value, langCodeIdxList)));

            csvWithoutHeader.AppendLine(qs);
            return csvWithoutHeader.ToString();
        }

        private string GetStringForValues(string value, List<int> langCodeIdxList)
        {
            var valuesList = value.Split(";").ToList();
            var stringForValuesList = Enumerable.Repeat(",", headerList.Count - HeadersLength).ToList();
            for (int i = 0; i < valuesList.Count; i++)
            {
                var idx = langCodeIdxList[i];
                stringForValuesList[idx] = valuesList[i];
            }
            for (int i = 0; i < stringForValuesList.Count; i++)
            {
                if (stringForValuesList[i] == ",")
                {
                    stringForValuesList[i] = string.Empty;
                }
            }
            return string.Join(",", stringForValuesList);
        }

        private int GetLanguageCodeIndex(string langCode)
        {
            var idxLangCode = headerList.FindIndex(x => x == langCode);
            if (idxLangCode != -1)
            {
                return idxLangCode - HeadersLength;
            }
            else
            {
                headerList.Add(langCode);
                return headerList.Count - HeadersLength - 1;
            }
        }
    }
}