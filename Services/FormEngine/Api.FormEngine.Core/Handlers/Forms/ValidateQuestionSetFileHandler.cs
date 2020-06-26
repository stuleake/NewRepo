using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.Constants;
using Api.FormEngine.Core.Managers;
using Api.FormEngine.Core.Services.Globals;
using Api.FormEngine.Core.Services.Parsers.Models;
using Api.FormEngine.Core.Services.Processors;
using Api.FormEngine.Core.Services.Validators;
using Api.FormEngine.Core.ViewModels;
using Api.FormEngine.Core.ViewModels.SheetModels;
using ExcelDataReader;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    ///  Handler to get error message in excel sheet
    /// </summary>
    public class ValidateQuestionSetFileHandler : IRequestHandler<ValidateQuestionSetFile, string>
    {
        private readonly FormsEngineContext formsEngineContext;
        private readonly IGlobalClient globalsClient;
        private readonly IConfiguration configuration;
        private readonly IValidator<ExcelSheetsData> excelSheetValidator;

        private readonly IProcessor<FieldConstraintParserModel> fieldConstraintProcessor;
        private readonly IProcessor<IEnumerable<Aggregations>> fieldAggregationProcessor;
        private readonly IProcessor<IEnumerable<Section>> sectionFieldMappingProcessor;
        private readonly IProcessor<QuestionSetParserModel> questionSetProcessor;
        private readonly IProcessor<SectionParserModel> sectionProcessor;
        private readonly IProcessor<SectionMappingParserModel> sectionMappingProcessor;
        private readonly IProcessor<FieldParserModel> fieldProcessor;
        private readonly IProcessor<TaxonomyParserModel> taxonomyProcessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateQuestionSetFileHandler"/> class.
        /// </summary>
        /// <param name="formsEngineContext">DbContext of the forms engine.</param>
        /// <param name="globalsClient">Accessor of global information.</param>
        /// <param name="configuration">Accessor of configuration information.</param>
        /// <param name="excelSheetValidator">Validator of excel sheets.</param>
        /// <param name="fieldAggregationProcessor">Processor of field aggregations.</param>
        /// <param name="sectionFieldMappingProcessor">Processor of section field mappings.</param>
        /// <param name="fieldConstraintProcessor">Processor of field constraints.</param>
        /// <param name="questionSetProcessor">Processor of question sets.</param>
        /// <param name="sectionProcessor">Processor of sections.</param>
        /// <param name="sectionMappingProcessor">Processor of section Mappings.</param>
        /// <param name="fieldProcessor">Processor of fields.</param>
        /// <param name="taxonomyProcessor">Processor of taxonomies.</param>
        public ValidateQuestionSetFileHandler(
            FormsEngineContext formsEngineContext,
            IGlobalClient globalsClient,
            IConfiguration configuration,
            IValidator<ExcelSheetsData> excelSheetValidator,
            IProcessor<IEnumerable<Aggregations>> fieldAggregationProcessor,
            IProcessor<IEnumerable<Section>> sectionFieldMappingProcessor,
            IProcessor<FieldConstraintParserModel> fieldConstraintProcessor,
            IProcessor<QuestionSetParserModel> questionSetProcessor,
            IProcessor<SectionParserModel> sectionProcessor,
            IProcessor<SectionMappingParserModel> sectionMappingProcessor,
            IProcessor<FieldParserModel> fieldProcessor,
            IProcessor<TaxonomyParserModel> taxonomyProcessor)
        {
            this.formsEngineContext = formsEngineContext;
            this.globalsClient = globalsClient;
            this.configuration = configuration;
            this.excelSheetValidator = excelSheetValidator;
            this.fieldProcessor = fieldProcessor;
            this.fieldConstraintProcessor = fieldConstraintProcessor;
            this.fieldAggregationProcessor = fieldAggregationProcessor;
            this.questionSetProcessor = questionSetProcessor;
            this.sectionProcessor = sectionProcessor;
            this.sectionMappingProcessor = sectionMappingProcessor;
            this.sectionFieldMappingProcessor = sectionFieldMappingProcessor;
            this.taxonomyProcessor = taxonomyProcessor;
        }

        private async Task SaveExcelSheetToDatabaseAsync(ExcelSheetsData sheetsData, string excelURL, string product, Guid userId)
        {
            var validationResult = excelSheetValidator.Validate(sheetsData);
            if (validationResult.IsValid)
            {
                await questionSetProcessor.ProcessAsync(new QuestionSetParserModel
                {
                    QuestionSet = sheetsData.QuestionSet[0],
                    ExcelUrl = excelURL,
                    Product = product,
                    UserId = userId
                }).ConfigureAwait(false);

                await sectionProcessor.ProcessAsync(new SectionParserModel
                {
                    QuestionSet = sheetsData.QuestionSet[0],
                    Dependencies = sheetsData.Dependencies,
                    UserId = userId
                }).ConfigureAwait(false);

                await sectionMappingProcessor.ProcessAsync(new SectionMappingParserModel
                {
                    QuestionSet = sheetsData.QuestionSet[0],
                    UserId = userId
                }).ConfigureAwait(false);

                await fieldProcessor.ProcessAsync(new FieldParserModel
                {
                    QuestionSet = sheetsData.QuestionSet[0],
                    Dependencies = sheetsData.Dependencies,
                    UserId = userId
                }).ConfigureAwait(false);

                await fieldConstraintProcessor.ProcessAsync(new FieldConstraintParserModel
                {
                    QuestionSet = sheetsData.QuestionSet[0],
                    Dependencies = sheetsData.Dependencies
                }).ConfigureAwait(false);

                await taxonomyProcessor.ProcessAsync(new TaxonomyParserModel
                {
                    QuestionSets = sheetsData.QuestionSet,
                    Taxonomies = TaxonomyHelper.GenerateTaxonomy(sheetsData)
                }).ConfigureAwait(true);

                await sectionFieldMappingProcessor.ProcessAsync(sheetsData.QuestionSet[0].Sections)
                    .ConfigureAwait(false);

                await fieldAggregationProcessor.ProcessAsync(sheetsData.Aggregations).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Execute excel sheets
        /// </summary>
        /// <param name="excelDataStream">Excel file data</param>
        /// <returns>Return excel response</returns>
        private async Task<ExcelResponse> ExecuteExcelSheetAsync(Stream excelDataStream, FormStructureData formSturcutreTypes)
        {
            var reader = ExcelReaderFactory.CreateReader(excelDataStream);
            var sheets = reader.AsDataSet().Tables;
            var excelResponse = await ExcelSheetManager.ExecuteAllExcelSheetsAsync(sheets, formSturcutreTypes, formsEngineContext).ConfigureAwait(false);
            return excelResponse;
        }

        private static async Task<Dictionary<string, string>> UploadFilesToBlobAsync(
            List<ExcelResponse> fileDetails,
            IGlobalClient globalsClient,
            ValidateQuestionSetFile fileContent,
            IConfiguration configuration)
        {
            bool saveFileToBlobFlag = false;
            var multicontent = new MultipartFormDataContent
            {
                { new ByteArrayContent(Encoding.ASCII.GetBytes(configuration["QSFileContainer"])), ApplicationConstants.ContainerName },
                { new ByteArrayContent(Encoding.ASCII.GetBytes(string.Empty)), ApplicationConstants.SubContainerName },
            };

            var metadata = new List<Dictionary<string, string>>();
            foreach (var file in fileDetails)
            {
                if (string.IsNullOrWhiteSpace(file.ErrorMessage))
                {
                    saveFileToBlobFlag = true;
                    metadata.Add(new Dictionary<string, string> { { ApplicationConstants.Description, string.Format(ApplicationConstants.BlobFileDescription, file.ExcelBlobFileName) } });
                    multicontent.Add(new StreamContent(file.ExcelFileStream), ApplicationConstants.Documents, file.ExcelBlobFileName);
                }
            }
            multicontent.Add(new ByteArrayContent(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(metadata))), ApplicationConstants.Metadatas);

            if (saveFileToBlobFlag)
            {
                var result = await globalsClient.UploadFileAsync(
                    multicontent,
                    configuration["ApiUri:Globals:DocumentUpload"],
                    fileContent.AuthToken).ConfigureAwait(false);
                return result.Value;
            }
            else
            {
                return new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Default Handler Method of the questions set file response
        /// </summary>
        /// <param name="request">file content request</param>
        /// <param name="cancellationToken">Cancellation Token</param>x
        /// <returns>return file response</returns>
        public async Task<string> Handle(ValidateQuestionSetFile request, CancellationToken cancellationToken)
        {
            if (request?.FileContent == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var sectionType = await formsEngineContext.SectionTypes.ToListAsync().ConfigureAwait(false);
            var fieldType = await formsEngineContext.FieldTypes.ToListAsync().ConfigureAwait(false);
            var displays = await formsEngineContext.Displays.ToListAsync().ConfigureAwait(false);
            var constraints = await formsEngineContext.Constraints.ToListAsync().ConfigureAwait(false);
            var answerType = await formsEngineContext.AnswerTypes.ToListAsync().ConfigureAwait(false);
            var rules = await formsEngineContext.Rules.ToListAsync().ConfigureAwait(false);
            var functions = await formsEngineContext.Functions.ToListAsync().ConfigureAwait(false);

            var formSturcutreTypes = new FormStructureData
            {
                SectionTypes = sectionType,
                FieldTypes = fieldType,
                Displays = displays,
                Constraints = constraints,
                AnswerTypes = answerType,
                Rules = rules,
                Functions = functions
            };

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var documentStream = request.FileContent.OpenReadStream();
            var excelResponseList = new List<ExcelResponse>();
            if (request.FileContent.FileName.EndsWith(".zip", System.StringComparison.OrdinalIgnoreCase))
            {
                var unzipExcelFile = new ZipArchive(documentStream, ZipArchiveMode.Read, true);
                foreach (var excelFileEntry in unzipExcelFile.Entries)
                {
                    if (excelFileEntry.Name.EndsWith(FormStructureConstants.Xlsx, StringComparison.OrdinalIgnoreCase))
                    {
                        var fileName = excelFileEntry.Name;
                        var excelFile = excelFileEntry.Open();
                        var memstream = new MemoryStream();
                        excelFile.CopyTo(memstream);
                        var response = await ExecuteExcelSheetAsync(memstream, formSturcutreTypes).ConfigureAwait(false);
                        response.FileName = excelFileEntry.Name;
                        response.ExcelFileStream = excelFileEntry.Open();
                        var fileNameWithoutExtension = fileName.Replace(FormStructureConstants.Xlsx, string.Empty, StringComparison.InvariantCultureIgnoreCase);
                        response.ExcelBlobFileName = $"{fileNameWithoutExtension}_{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}{FormStructureConstants.Xlsx}";
                        excelResponseList.Add(response);
                    }
                    else
                    {
                        var fileIsNotExcelErrorMessage = string.Format(ApplicationConstants.NonExecutableFileFound, excelFileEntry.Name);
                        excelResponseList.Add(new ExcelResponse { ErrorMessage = fileIsNotExcelErrorMessage, FileName = excelFileEntry.Name });
                    }
                }
            }
            else if (request.FileContent.FileName.EndsWith(FormStructureConstants.Xlsx, System.StringComparison.OrdinalIgnoreCase))
            {
                var response = await ExecuteExcelSheetAsync(documentStream, formSturcutreTypes).ConfigureAwait(false);
                var fileNameWithoutExtension = request.FileContent.FileName.Replace(FormStructureConstants.Xlsx, string.Empty, StringComparison.InvariantCultureIgnoreCase);
                response.FileName = request.FileContent.FileName;
                response.ExcelFileStream = request.FileContent.OpenReadStream();
                response.ExcelBlobFileName = $"{fileNameWithoutExtension}_{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}{FormStructureConstants.Xlsx}";
                excelResponseList.Add(response);
            }
            else
            {
                var fileIsNotExcelErrorMessage = string.Format(ApplicationConstants.NonExecutableFileFound, request.FileContent.FileName);
                excelResponseList.Add(new ExcelResponse { ErrorMessage = fileIsNotExcelErrorMessage, FileName = request.FileContent.FileName });
            }

            var blobFileUrl = await UploadFilesToBlobAsync(excelResponseList, globalsClient, request, configuration).ConfigureAwait(false);
            List<QuestionSetFileResponse> errors = new List<QuestionSetFileResponse>();
            foreach (var excelResponse in excelResponseList)
            {
                if (string.IsNullOrWhiteSpace(excelResponse.ErrorMessage))
                {
                    if (blobFileUrl != null && blobFileUrl.Keys.Contains(excelResponse.ExcelBlobFileName))
                    {
                        var excelFileUrl = blobFileUrl[excelResponse.ExcelBlobFileName];
                        await SaveExcelSheetToDatabaseAsync(
                            excelResponse.FormattedExcelSheetData, excelFileUrl, request.Product, request.UserId).ConfigureAwait(false);
                    }
                    else
                    {
                        var error = new List<string> { FormStructureConstants.FileNotAddedToBlob };
                        errors.Add(new QuestionSetFileResponse { Error = error, FileName = excelResponse.ExcelBlobFileName });
                    }
                }
                else
                {
                    var error = excelResponse.ErrorMessage.Replace(Environment.NewLine, ";", StringComparison.InvariantCulture);
                    errors.Add(new QuestionSetFileResponse { Error = error.Split(';').ToList(), FileName = excelResponse.FileName });
                }
            }

            if (errors.Any())
            {
                throw new TQException(JsonConvert.SerializeObject(errors));
            }

            return string.Empty;
        }
    }
}