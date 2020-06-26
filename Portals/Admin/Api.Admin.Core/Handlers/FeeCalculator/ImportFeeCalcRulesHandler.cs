using Api.Admin.Core.Commands.FeeCalculator;
using Api.Admin.Core.Services.FeeCalculator;
using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;

namespace Api.Admin.Core.Handlers.FeeCalculator
{
    /// <summary>
    /// Handler class to import and save Fee calculator Rules
    /// </summary>
    public class ImportFeeCalcRulesHandler : IRequestHandler<ImportFeeCalculationRules, bool>
    {
        private readonly IConfiguration configuration;
        private readonly IFeeCalculatorClient feecalcClient;
        private const string Json = ".json";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportFeeCalcRulesHandler"/> class.
        /// </summary>
        /// <param name="configuration">Configuration data.</param>
        /// <param name="feecalcClient">Fee calculator client</param>
        public ImportFeeCalcRulesHandler(IConfiguration configuration, IFeeCalculatorClient feecalcClient)
        {
            this.configuration = configuration;
            this.feecalcClient = feecalcClient;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(ImportFeeCalculationRules request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var sendRequest = await ParserZipFileAsync(request.File).ConfigureAwait(false);
            sendRequest.FileName = request.File.FileName;
            sendRequest.UserId = request.UserId;
            var response = await feecalcClient.ImportFeeCalculatorRulesAsync(
                        configuration["ApiUri:FeeCalculator:ImportFeeCalcRules"],
                        JObject.Parse(JsonConvert.SerializeObject(sendRequest)),
                        request.Country,
                        request.AuthToken).ConfigureAwait(false);

            if (response == null)
            {
                throw new ServiceException("Null response received from the fee calculator");
            }
            if (response.Code == (int)HttpStatusCode.BadRequest)
            {
                throw new TQException(response.Message);
            }

            if (response.Code != (int)HttpStatusCode.Created)
            {
                throw new ServiceException($"Erroneous response received from the fee calculator ==> {response.Code} - {response.Message}");
            }

            return response.Value;
        }

        private static async Task<FeeCalculatorRulesDetails> ParserZipFileAsync(IFormFile file)
        {
            Dictionary<string, string> masterParams;
            var rules = new List<RuleDefModel>();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream).ConfigureAwait(false);

            using var archive = new ZipArchive(stream, ZipArchiveMode.Update, false);
            var ruleDefs = archive.Entries.Where(x => x.FullName.EndsWith(".js", StringComparison.OrdinalIgnoreCase)).ToList();

            var masterParamFile = archive.Entries.FirstOrDefault(x => x.Name.Equals("MasterParameters.json", StringComparison.OrdinalIgnoreCase));

            masterParams = await ParserMasterParametersAsync(masterParamFile).ConfigureAwait(false);

            foreach (var ruleData in archive.Entries.Where(x =>
            x.FullName.EndsWith(Json, StringComparison.OrdinalIgnoreCase)))
            {
                using var zipstream = ruleData.Open();
                using MemoryStream ms = new MemoryStream();

                try
                {
                    zipstream.CopyTo(ms);
                    ms.Position = 0;
                    var reader = new StreamReader(ms);
                    var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" };
                    var jsonScenario = reader.ReadToEnd();
                    RuleDefModel rule = JsonConvert.DeserializeObject<RuleDefModel>(jsonScenario, dateTimeConverter);
                    var def = await ParseJsRuleDefinitionAsync(ruleDefs, ruleData.Name).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(def))
                    {
                        rule.Definition = def;
                        rules.Add(rule);
                        zipstream.Close();
                    }
                }
                catch
                {
                    throw new TQException($"Invalid content in Rule file with Name : {ruleData.Name}");
                }
            }
            return new FeeCalculatorRulesDetails
            {
                Rules = rules,
                MasterParameters = masterParams
            };
        }

        private static async Task<string> ParseJsRuleDefinitionAsync(List<ZipArchiveEntry> ruleDefs, string fileName)
        {
            fileName = fileName.Replace(Json, string.Empty, StringComparison.OrdinalIgnoreCase) + ".js";
            var def = ruleDefs.FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.OrdinalIgnoreCase));
            using var zipstream = def.Open();
            using MemoryStream ms = new MemoryStream();
            try
            {
                await zipstream.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                var reader = new StreamReader(ms);
                var stringDef = reader.ReadToEnd();
                zipstream.Close();
                return stringDef;
            }
            catch
            {
                throw new TQException($"Invalid content in Rule file definition with Name : {def.Name}");
            }
        }

        private static async Task<Dictionary<string, string>> ParserMasterParametersAsync(ZipArchiveEntry masterFile)
        {
            if (masterFile == null)
            {
                return null;
            }
            Dictionary<string, string> dict = null;
            using var zipstream = masterFile.Open();
            using MemoryStream ms = new MemoryStream();
            try
            {
                await zipstream.CopyToAsync(ms).ConfigureAwait(false);
                ms.Position = 0;
                var reader = new StreamReader(ms);
                var stringDef = reader.ReadToEnd();
                dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(stringDef);
                zipstream.Close();
            }
            catch
            {
                throw new TQException($"Invalid master parameter data");
            }
            masterFile.Delete();
            return dict;
        }
    }
}