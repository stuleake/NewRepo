using Api.FormEngine.Core.Commands.Forms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;
using TQ.Data.FormEngine.Schemas.Sessions;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Create response of question set
    /// </summary>
    public class CreateQuestionSetResponseHandler : IRequestHandler<CreateQuestionSetResponse, Guid>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateQuestionSetResponseHandler"/> class
        /// </summary>
        /// <param name="formsEngineContext">object of formsEngineContext being passed using dependency injection</param>
        public CreateQuestionSetResponseHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public async Task<Guid> Handle(CreateQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null || request.Response == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IDbContextTransaction transaction = formsEngineContext.Database.BeginTransaction();
            try
            {
                // looks for the application you are trying to create.
                var application = await formsEngineContext.QSCollection.FirstOrDefaultAsync(ua => ua.ApplicationName.ToLower() == request.ApplicationName.ToLower()
                && ua.UserId.Equals(request.UserId) && ua.CountryCode.ToLower() == request.Country.ToLower()).ConfigureAwait(false);

                var questionSet = await formsEngineContext.QS.FirstOrDefaultAsync(qs => qs.QSId.Equals(request.QuestionSetId)).ConfigureAwait(false);

                if (questionSet == null)
                {
                    throw new TQException("Invalid question set Id.");
                }
                var requestAnswersJson = JsonConvert.DeserializeObject<Dictionary<string, object>>(request.Response);
                var requestAnswers = requestAnswersJson.Where(req => !string.IsNullOrEmpty(req.Value.ToString())).ToDictionary(req => req.Key, req => req.Value.ToString().Trim());

                var fieldDict = requestAnswers.ToDictionary(ans => ans.Key.Substring(0, 36), ans => ans.Value);

                var fieldtypes = await formsEngineContext.Field
                               .Join(formsEngineContext.FieldTypes, field => field.FieldTypeId, type => type.FieldTypeId, (field, type) => new
                               {
                                   field.FieldId,
                                   type.FieldTypes
                               })
                                .Where(f => fieldDict.Keys.Contains(f.FieldId.ToString())).ToListAsync().ConfigureAwait(false);

                if (fieldtypes.Any())
                {
                    bool isfieldvalid = true;
                    var fielderror = new StringBuilder();
                    foreach (var field in fieldDict)
                    {
                        var fieldtype = fieldtypes.Where(ft => ft.FieldId == new Guid(field.Key)).Select(ft => ft.FieldTypes);
                        bool enumTypeExists = Enum.TryParse<FieldTypes>(fieldtype.FirstOrDefault(), true, out FieldTypes fieldType);
                        if (enumTypeExists)
                        {
                            switch (fieldType)
                            {
                                case FieldTypes.Number:
                                    if (!double.TryParse(field.Value, out double num))
                                    {
                                        isfieldvalid = false;
                                        fielderror.Append(field.Key).Append(" ");
                                    }
                                    break;

                                case FieldTypes.NumberSelector:
                                    if (!int.TryParse(field.Value, out int number))
                                    {
                                        isfieldvalid = false;
                                        fielderror.Append(field.Key).Append(" ");
                                    }
                                    break;

                                case FieldTypes.Date:
                                    if (!DateTime.TryParse(field.Value, CultureInfo.CreateSpecificCulture("en-GB"), DateTimeStyles.None, out DateTime date))
                                    {
                                        isfieldvalid = false;
                                        fielderror.Append(field.Key).Append(" ");
                                    }
                                    break;
                            }
                        }
                    }
                    if (!isfieldvalid)
                    {
                        fielderror.Append(" : ").Append(FieldErrorMessageConstants.FieldDataType);
                        throw new TQException(fielderror.ToString());
                    }
                }

                // if no application found.
                if (application == null)
                {
                    var collectionType = await formsEngineContext.QSCollectionType.FirstOrDefaultAsync(x => x.QSCollectionTypeId.Equals(request.ApplicationTypeId)).ConfigureAwait(false);

                    if (collectionType == null)
                    {
                        throw new TQException("Invalid Application type.");
                    }

                    // create a  new application.
                    application = new QSCollection
                    {
                        ApplicationName = request.ApplicationName,
                        QSCollectionTypeId = collectionType.QSCollectionTypeId,
                        CreatedDate = DateTime.UtcNow,
                        LastModifiedDate = DateTime.Now,
                        LastModifiedBy = request.UserId,
                        UserId = request.UserId,
                        CountryCode = request.Country,
                    };
                    formsEngineContext.QSCollection.Add(application);
                    await formsEngineContext.SaveChangesAsync().ConfigureAwait(false);
                }
                var qsr = await formsEngineContext.Qsr.FirstOrDefaultAsync(qr => qr.QSCollectionId.Equals(application.QSCollectionId)
                     && qr.QSId.Equals(request.QuestionSetId)).ConfigureAwait(false);

                if (qsr != null)
                {
                    var existingAnswers = await formsEngineContext.QsrAnswer.Where(x => x.QsrId.Equals(qsr.QsrId)).ToListAsync().ConfigureAwait(false);
                    foreach (var data in requestAnswers)
                    {
                        var fieldData = data.Key.LastIndexOf('-');
                        var currentAns = existingAnswers != null && existingAnswers.Count
                            > 0 ? existingAnswers.FirstOrDefault(x => x.FieldNo.Equals(Convert.ToInt32(data.Key.Substring(fieldData + 1)))) : null;
                        if (currentAns != null)
                        {
                            currentAns.FieldId = Guid.Parse(data.Key.Substring(0, fieldData));
                            currentAns.FieldNo = Convert.ToInt32(data.Key.Substring(fieldData + 1));
                            currentAns.Answer = data.Value;
                            currentAns.LastModifiedBy = request.UserId;
                            currentAns.LastModifiedDate = DateTime.UtcNow;
                        }
                        else
                        {
                            formsEngineContext.QsrAnswer.Add(new QsrAnswer
                            {
                                FieldId = Guid.Parse(data.Key.Substring(0, fieldData)),
                                FieldNo = Convert.ToInt32(data.Key.Substring(fieldData + 1)),
                                Answer = data.Value,
                                QsrId = qsr.QsrId,
                                CreatedDate = DateTime.UtcNow,
                                LastModifiedDate = DateTime.UtcNow,
                                LastModifiedBy = request.UserId
                            });
                        }
                    }
                }
                else
                {
                    qsr = new Qsr
                    {
                        QSCollectionId = application.QSCollectionId,
                        QSId = request.QuestionSetId,
                        QSNo = questionSet.QSNo,
                        QSVersion = questionSet?.QSVersion.ToString(),
                        LastModifiedBy = request.UserId,
                        LastModifiedDate = DateTime.UtcNow
                    };

                    formsEngineContext.Qsr.Add(qsr);

                    foreach (var data in requestAnswers)
                    {
                        var fieldData = data.Key.LastIndexOf('-');

                        formsEngineContext.QsrAnswer.Add(new QsrAnswer
                        {
                            FieldId = Guid.Parse(data.Key.Substring(0, fieldData)),
                            FieldNo = Convert.ToInt32(data.Key.Substring(fieldData + 1)),
                            Answer = data.Value,
                            QsrId = qsr.QsrId,
                            CreatedDate = DateTime.UtcNow,
                            LastModifiedDate = DateTime.UtcNow,
                            LastModifiedBy = request.UserId
                        });
                    }
                }

                await formsEngineContext.SaveChangesAsync().ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);

                return application.QSCollectionId;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                await transaction.DisposeAsync();
            }
        }
    }
}