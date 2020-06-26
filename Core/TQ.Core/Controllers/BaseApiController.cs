using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace TQ.Core.Controllers
{
    /// <summary>
    /// Base Api
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Gets the Office365 User Id from the Authorization token.
        /// </summary>
        protected Guid CurrentUser
        {
            get
            {
                string userId = string.Empty;
                var userUpnClaim = HttpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (userUpnClaim != null)
                {
                    userId = userUpnClaim.Value;
                }

                return Guid.Parse(userId);
            }
        }

        /// <summary>
        /// Gets the Roles of the User.
        /// </summary>
        protected string Roles => HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
    }
}