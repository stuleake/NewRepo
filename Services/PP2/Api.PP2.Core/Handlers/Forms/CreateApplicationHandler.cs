using Api.PP2.Core.Commands.Forms;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.PlanningPortal;
using TQ.Data.PlanningPortal.Schemas.Dbo;

namespace Api.PP2.Core.Handlers.Forms
{
    /// <summary>
    /// Handler to Create Application set response
    /// </summary>
    public class CreateApplicationHandler : IRequestHandler<CreateApplication, Guid>
    {
        private readonly PlanningPortalContext planningPortalContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateApplicationHandler"/> class.
        /// </summary>
        /// <param name="planningPortalContext">object of planningPortalContext being passed using dependency injection</param>
        public CreateApplicationHandler(PlanningPortalContext planningPortalContext)
        {
            this.planningPortalContext = planningPortalContext;
        }

        /// <inheritdoc/>
        public async Task<Guid> Handle(CreateApplication request, CancellationToken cancellationToken)
        {
            var application = await planningPortalContext.UserApplications.FirstOrDefaultAsync(app => app.UserApplicationId.Equals(request.ApplicationId)).ConfigureAwait(false);

            if (application == null)
            {
                var app = new UserApplication
                {
                    UserId = request.UserId,
                    ApplicationName = request.ApplicationName,
                    UserApplicationId = request.ApplicationId
                };
                app.LastSaved = DateTime.UtcNow;
                app.Status = "Submitted";

                planningPortalContext.UserApplications.Add(app);

                await planningPortalContext.SaveChangesAsync().ConfigureAwait(false);
            }

            return request.ApplicationId;
        }
    }
}