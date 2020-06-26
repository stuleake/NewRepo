using Api.Admin.Core.Commands.ApplicationList;
using Api.Admin.Core.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TQ.Core.Constants;
using TQ.Core.Controllers;
using TQ.Core.Enums;
using TQ.Core.Exceptions;
using TQ.Core.Filters;

namespace Api.Admin.Controllers
{
    /// <summary>
    /// Api to list all the application for a user
    /// </summary>
    [Route("api/[controller]")]
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
        /// <param name="country">country as a string</param>
        /// <param name="authorizationToken">Authorization Token from request headers.</param>
        /// <param name="impersonate">Impersonate request header.</param>
        /// <param name="impersonateUserId">Impersonate UserId request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Route("drafts/{country}")]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<ApplicationListModel>> GetDraftApplicationsAsync(
            string country,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Impersonate)]bool impersonate,
            [FromHeader(Name = RequestHeaderConstants.ImpersonatedUserId)] Guid? impersonateUserId)
        {
            Guid userId = GetUserId(ref country, impersonate, impersonateUserId);

            ApplicationListRequestModel request = new ApplicationListRequestModel
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
        /// <param name="country">country as a string</param>
        /// <param name="authorizationToken">Authorization Token from request headers.</param>
        /// <param name="impersonate">Impersonate request header.</param>
        /// <param name="impersonateUserId">Impersonate UserId request header.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpGet]
        [Route("submitted/{country}")]
        [CustomAuthorize(
            RoleTypes.PP2SuperAdmin,
            RoleTypes.SupportUser,
            RoleTypes.LpaAdmin,
            RoleTypes.LpaUser,
            RoleTypes.StandardUser,
            RoleTypes.OrganisationAdmin,
            RoleTypes.OrganisationUser)]
        public async Task<ActionResult<ApplicationListModel>> GetSubmittedApplicationsAsync(
            string country,
            [FromHeader(Name = RequestHeaderConstants.AuthorizationToken)]string authorizationToken,
            [FromHeader(Name = RequestHeaderConstants.Impersonate)]bool impersonate,
            [FromHeader(Name = RequestHeaderConstants.ImpersonatedUserId)] Guid? impersonateUserId)
        {
            Guid userId = GetUserId(ref country, impersonate, impersonateUserId);

            ApplicationListRequestModel request = new ApplicationListRequestModel
            {
                Type = ApplicationStatusConstants.Submitted,
                AuthToken = authorizationToken,
                Country = country,
                UserId = userId
            };

            var response = await mediator.Send(request);
            return response;
        }

        private Guid GetUserId(ref string country, bool impersonate, Guid? impersonateUserId)
        {
            var userId = impersonate && impersonateUserId.HasValue ? impersonateUserId.Value : this.CurrentUser;
            country = (country ?? string.Empty).ToUpper();
            if (country != CountryConstants.England && country != CountryConstants.Wales)
            {
                throw new TQException("Invalid Country name");
            }

            return userId;
        }
    }
}