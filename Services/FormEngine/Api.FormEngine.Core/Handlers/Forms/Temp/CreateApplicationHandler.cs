using Api.FormEngine.Core.Commands.Forms;
using AutoMapper;
using CT.KeyVault;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;
using TQ.Data.PlanningPortal;
using TQ.Data.PlanningPortal.Schemas.Dbo;

namespace Api.FormEngine.Core.Handlers.Forms
{
    /// <summary>
    /// Handler that manages  Create Application.
    /// </summary>
    public class CreateApplicationHandler : BaseHandler, IRequestHandler<CreateApplication, Guid>
    {
        /// <summary>
        ///  Initializes a new instance of the <see cref="CreateApplicationHandler"/> class.
        /// </summary>
        /// <param name="keyvault">keyvault</param>
        /// <param name="config">config</param>
        /// <param name="mapper">mapper</param>
        public CreateApplicationHandler(IVaultManager keyvault, IConfiguration config, IMapper mapper) : base(keyvault, config)
        {
        }

        /// <summary>
        /// Manages method to Create Application.
        /// </summary>
        /// <param name="request">Application object</param>
        /// <returns>guid</returns>
        public async Task<Guid> CreateApplicationAsync(CreateApplication request)
        {
            try
            {
                PlanningPortalContext planningPortalContext = await GetContextAsync(request).ConfigureAwait(false);
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <inheritdoc/>

        public Task<Guid> Handle(CreateApplication request, CancellationToken cancellationToken)
        {
            return CreateApplicationAsync(request);
        }
    }
}