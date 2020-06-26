using Api.FormEngine.Core.Commands.Forms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Delete response of question set
    /// </summary>
    public class DeleteQuestionSetResponseHandler : IRequestHandler<DeleteQuestionSetResponse, bool>
    {
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteQuestionSetResponseHandler"/> class
        /// </summary>
        /// <param name="formsEngineContext">object of formsEngineContext being passed using dependency injection</param>
        public DeleteQuestionSetResponseHandler(FormsEngineContext formsEngineContext)
        {
            this.formsEngineContext = formsEngineContext;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(DeleteQuestionSetResponse request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            IDbContextTransaction transaction = formsEngineContext.Database.BeginTransaction();
            try
            {
                var qsrModel = await formsEngineContext.Qsr.FirstOrDefaultAsync(qsr => qsr.QSCollectionId.Equals(request.Id)).ConfigureAwait(false);

                var qsrAnswer = await formsEngineContext.QsrAnswer.Where(x => x.QsrId.Equals(qsrModel.QsrId)).ToListAsync().ConfigureAwait(false);
                formsEngineContext.QsrAnswer.RemoveRange(qsrAnswer);
                await formsEngineContext.SaveChangesAsync().ConfigureAwait(false);

                formsEngineContext.Qsr.Remove(qsrModel);
                await formsEngineContext.SaveChangesAsync().ConfigureAwait(false);

                var qsrcollection = await formsEngineContext.QSCollection.FirstOrDefaultAsync(qsr => qsr.QSCollectionId.Equals(request.Id)).ConfigureAwait(false);
                formsEngineContext.QSCollection.Remove(qsrcollection);
                await formsEngineContext.SaveChangesAsync().ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);

                return true;
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