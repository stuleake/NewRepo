using Api.Planner.Core.Commands.ApplicationList;
using Api.Planner.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Filters;

namespace Api.Planner.Controllers
{
    /// <summary>
    /// Api to list all the application for a user
    /// </summary>
    [ApiController]
    public class ApplicationListController : BaseApiController
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationListController"/> class.
        /// </summary>
        /// <param name="mediator">object of mediatR being passed using dependency injection</param>
        public ApplicationListController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get method to list draft application for a user.
        /// </summary>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="country">Country request header.</param>
        /// <param name="impersonate">Impersonate request header.</param>
        /// <param name="impersonateUserId">ImpersonateUserId request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Route("drafts")]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<ApplicationListModel>> GetDraftApplicationsAsync(
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country,
            [FromHeader(Name = RequestHeaderConstants.Impersonate)]bool impersonate,
            [FromHeader(Name = RequestHeaderConstants.ImpersonatedUserId)]Guid? impersonateUserId)
        {
            var userId = GetUser(impersonate, impersonateUserId);

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Drafts,
                AuthToken = authorizationToken,
                Country = country,
                UserId = userId
            };
            var response = await mediator.Send(request);
            return response;
        }

        /// <summary>
        /// Get method to list submitted application for a user.
        /// </summary>
        /// <param name="authorizationToken">Authorization token request header.</param>
        /// <param name="country">Country request header.</param>
        /// <param name="impersonate">Impersonate request header.</param>
        /// <param name="impersonateUserId">ImpersonateUserId request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Route("submitted")]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<ApplicationListModel>> GetSubmittedApplicationsAsync(
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Country)]string country,
            [FromHeader(Name = RequestHeaderConstants.Impersonate)]bool impersonate,
            [FromHeader(Name = RequestHeaderConstants.ImpersonatedUserId)]Guid? impersonateUserId)
        {
            var userId = GetUser(impersonate, impersonateUserId);

            var request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Submitted,
                AuthToken = authorizationToken,
                Country = country,
                UserId = userId
            };
            var response = await mediator.Send(request);
            return response;
        }

        private Guid GetUser(bool impersonate, Guid? impersonateUserId)
        {
            var userId = impersonate && impersonateUserId.HasValue ? impersonateUserId.Value : this.CurrentUser;
            return userId;
        }
    }
}