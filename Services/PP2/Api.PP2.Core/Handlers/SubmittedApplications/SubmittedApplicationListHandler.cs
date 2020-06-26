using Api.PP2.Core.Commands.SubmittedApplications;
using Api.PP2.Core.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Data.PlanningPortal;

namespace Api.PP2.Core.Handlers.SubmittedApplications
{
    /// <summary>
    /// Class to list of Application submitted
    /// </summary>
    public class SubmittedApplicationListHandler : IRequestHandler<SubmittedApplicationsRequestModel, ApplicationListModel>
    {
        private readonly IMapper mapper;
        private readonly PlanningPortalContext planningPortalContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmittedApplicationListHandler"/> class.
        /// </summary>
        /// <param name="mapper">object of mapper</param>
        /// <param name="planningPortalContext">object of planningPortalContext</param>
        public SubmittedApplicationListHandler(IMapper mapper, PlanningPortalContext planningPortalContext)
        {
            this.mapper = mapper;
            this.planningPortalContext = planningPortalContext;
        }

        /// <inheritdoc/>
        public async Task<ApplicationListModel> Handle(SubmittedApplicationsRequestModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ApplicationListModel { Type = ApplicationStatusConstants.Submitted };

            List<TQ.Data.PlanningPortal.Schemas.Dbo.UserApplication> applications = new List<TQ.Data.PlanningPortal.Schemas.Dbo.UserApplication>();

            if ((request.Portal ?? string.Empty).ToUpperInvariant() == PortalConstants.Planner)
            {
                applications = await planningPortalContext.UserApplications.Where(ua => ua.UserId.Equals(request.UserId))
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
            else if ((request.Portal ?? string.Empty).ToUpperInvariant() == PortalConstants.Admin)
            {
                applications = await planningPortalContext.UserApplications
                    .ToListAsync()
                    .ConfigureAwait(false);
            }

            response.Applications = mapper.Map<List<TQ.Data.PlanningPortal.Schemas.Dbo.UserApplication>, List<ViewModels.UserApplication>>(applications);
            await planningPortalContext.DisposeAsync();

            return response;
        }
    }
}