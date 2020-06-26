using Api.FormEngine.Core.Commands.Forms;
using Api.FormEngine.Core.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Exceptions;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Class for the TaxonomiesByApplicationType handler
    /// </summary>
    public class GetTaxonomiesByApplicationTypeHandler : IRequestHandler<GetTaxonomiesByApplicationType, List<QuestionSetWithTaxonomies>>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTaxonomiesByApplicationTypeHandler"/> class.
        /// </summary>
        /// <param name="formsEngineContext">Object of forms-engine context</param>
        public GetTaxonomiesByApplicationTypeHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <summary>
        /// Handler for get taxonomies by application number
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Returns all taxonomies</returns>
        public async Task<List<QuestionSetWithTaxonomies>> Handle(GetTaxonomiesByApplicationType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = await formsEngineContext.QSCollectionType.Where(qs => qs.ApplicationTypeRefNo == request.ApplicationTypeRefNo &&
                                                qs.Tenant.ToLower() == request.Country.ToLower() && qs.Product.ToLower() == request.Product.ToLower())
                                                .OrderByDescending(ver => ver.QSCollectionVersion).Take(1)
                                                .Join(
                                                    formsEngineContext.QSCollectionMapping,
                                                    qsc => qsc.QSCollectionTypeId,
                                                    qsmap => qsmap.QSCollectionTypeId,
                                                    (qsc, qsmap) => new
                                                    {
                                                        QSMap = qsmap
                                                    })
                                                .Join(
                                                    formsEngineContext.QS.Where(qs => qs.StatusId == 2 && qs.Tenant.ToLower() == request.Country.ToLower() &&
                                                qs.Product.ToLower() == request.Product.ToLower()),
                                                    qsmap => qsmap.QSMap.QSId,
                                                    qs => qs.QSId,
                                                    (qsmap, qs) => new
                                                    {
                                                        QS = qs
                                                    })
                                                .Join(
                                                    formsEngineContext.Taxonomy.Where(taxo => taxo.LanguageCode.ToLower() == request.Language.ToLower() &&
                                                taxo.Tenant.ToLower() == request.Country.ToLower() && taxo.Product.ToLower() == request.Product.ToLower()),
                                                    qstaxomap => new { Id = qstaxomap.QS.QSNo, Version = Convert.ToString(qstaxomap.QS.QSVersion) },
                                                    taxo => new { Id = taxo.QsNo, Version = taxo.QsVersion },
                                                    (qstaxomap, taxo) => new QuestionSetWithTaxonomies
                                                    {
                                                        QSName = qstaxomap.QS.QSName,
                                                        QSNo = taxo.QsNo,
                                                        QSVersion = taxo.QsVersion,
                                                        QSTaxonomy = JsonConvert.DeserializeObject<Dictionary<string, string>>(taxo.TaxonomyDictionary)
                                                    }).ToListAsync().ConfigureAwait(false);

            if (!response.Any())
            {
                throw new TQException($"No matching records found in database!");
            }
            return response;
        }
    }
}