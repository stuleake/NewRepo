using Api.FormEngine.Core.Commands.DraftApplications;
using Api.FormEngine.Core.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Data.FormEngine;

namespace Api.FormEngine.Core.Handlers.DraftApplications
{
    /// <summary>
    /// A handler class to get all the draft application for a user.
    /// </summary>
    public class DraftApplicationListHandler : IRequestHandler<DraftApplicationListRequestModel, ApplicationListModel>
    {
        private readonly IMapper mapper;
        private readonly FormsEngineContext formsEngineContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DraftApplicationListHandler"/> class.
        /// </summary>
        /// <param name="formsEngineContext">object of form engine context being passed using dependency injection</param>
        /// <param name="mapper">object of mapper being passed using dependency injection</param>
        public DraftApplicationListHandler(FormsEngineContext formsEngineContext, IMapper mapper)
        {
            this.formsEngineContext = formsEngineContext;
            this.mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<ApplicationListModel> Handle(DraftApplicationListRequestModel request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var response = new ApplicationListModel { Type = ApplicationStatusConstants.Drafts };
            var apps = new List<TQ.Data.FormEngine.Schemas.Sessions.QSCollection>();

            if ((request.Portal ?? string.Empty).ToUpperInvariant() == PortalConstants.Planner)
            {
                apps = await formsEngineContext.QSCollection
                        .Where(qs => qs.UserId.Equals(request.UserId) && qs.CountryCode.ToLower() == request.Country.ToLower())
                        .ToListAsync()
                        .ConfigureAwait(false);
            }
            else if ((request.Portal ?? string.Empty).ToUpperInvariant() == PortalConstants.Admin)
            {
                apps = await formsEngineContext.QSCollection
                        .Where(qs => qs.CountryCode.ToLower() == request.Country.ToLower())
                        .ToListAsync()
                        .ConfigureAwait(false);
            }

            response.Applications = mapper.Map<List<TQ.Data.FormEngine.Schemas.Sessions.QSCollection>, List<UserApplication>>(apps);

            return response;
        }
    }
}