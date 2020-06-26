using Api.FormEngine.Core.Commands.Forms.QuestionSet;
using Api.FormEngine.Core.Enums;
using Api.FormEngine.Core.ViewModels;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to retrieve Question Set by Application Type
    /// </summary>
    public class GetQuestionSetByApplicationTypeHandler : IRequestHandler<GetByApplicationType, List<QuestionSet>>
    {
        private readonly FormsEngineContext frmEnginecontext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetQuestionSetByApplicationTypeHandler"/> class.
        /// </summary>
        /// <param name="frmEnginecontext">Form Engine DB Context Object being passed from DI</param>
        public GetQuestionSetByApplicationTypeHandler(FormsEngineContext frmEnginecontext)
        {
            this.frmEnginecontext = frmEnginecontext;
        }

        /// <inheritdoc/>
        public async Task<List<QuestionSet>> Handle(GetByApplicationType request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var results = (from questionSetMappingCollection in frmEnginecontext.QSCollectionMapping
                          join questionSet in frmEnginecontext.QS on questionSetMappingCollection.QSId equals questionSet.QSId
                          join subQuery in from subQuestionSet in frmEnginecontext.QS
                                           join subQuestionSetMappingCollection in frmEnginecontext.QSCollectionMapping on
                                           new { QuestionNumber = subQuestionSet.QSNo } equals
                                           new { QuestionNumber = subQuestionSetMappingCollection.QSNo }
                                           where subQuestionSetMappingCollection.QSCollectionTypeId == request.QSCollectionTypeId && subQuestionSet.StatusId == (int)FormsStatus.Active
                                           group subQuestionSet by subQuestionSet.QSNo into grp
                                           select new
                                           {
                                               QuestionNumber = grp.Key,
                                               QuestionVersion = grp.Max(p => p.QSVersion)
                                           } on new { Version = questionSet.QSVersion, QuestionNumber = questionSet.QSNo } equals
                              new { Version = subQuery.QuestionVersion, QuestionNumber = subQuery.QuestionNumber }
                          where questionSetMappingCollection.QSCollectionTypeId == request.QSCollectionTypeId
                                && questionSet.StatusId == (int)FormsStatus.Active
                          orderby questionSetMappingCollection.Sequence
                          select new QuestionSet
                          {
                              QSId = questionSetMappingCollection.QSId,
                              QSName = questionSet.QSName,
                              QSNo = questionSetMappingCollection.QSNo,
                              Sequence = questionSetMappingCollection.Sequence,
                              QSVersion = questionSet.QSVersion
                          }).ToList();

            return results;
        }
    }
}