using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using TQ.Core.Enums;
using TQ.Core.Helpers;

namespace TQ.Core.Filters
{
    /// <summary>
    /// Custom Authorize Attribute
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.All)]
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly List<string> apiRoles;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="roles">roles</param>
        public CustomAuthorizeAttribute(params RoleTypes[] roles)
        {
            this.apiRoles = roles.Select(x => x.ToString().ToLower()).ToList();
        }

        /// <summary>
        /// Authorize on filter context
        /// </summary>
        /// <param name="context">filter context</param>
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            // check for token authentication
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // bypass this custom auth handling if api is not role specific
            if (apiRoles == null || apiRoles.Contains(RoleTypes.AllUsers.ToString()))
            {
                return;
            }

            // current user's roles
            string userClaims = context.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            if (!string.IsNullOrEmpty(context.HttpContext.Request.Headers["ImpersonatedUserId"]))
            {
                var scope = context.HttpContext.RequestServices;
                var client = (B2CGraphClient)scope.GetService(typeof(B2CGraphClient));

                var impUserId = context.HttpContext.Request.Headers["ImpersonatedUserId"];
                var user = client.GetUserByObjectIdAsync(impUserId).Result;
                Regex regex = new Regex("extension_[A-Za-z0-9]*_roles\":\"[A-Za-z]*");
                var impUserRegexValue = regex.Match(user).Value;

                // Impersonated user's claim
                userClaims = impUserRegexValue.Split('"').Last();
            }

            if (string.IsNullOrEmpty(userClaims))
            {
                context.Result = new ForbidResult();
                return;
            }
            var userRoles = userClaims.Split(',');

            foreach (var role in userRoles)
            {
                if (apiRoles.Contains(role.ToLower()))
                {
                    return;
                }
            }

            context.Result = new ForbidResult();
        }
    }
}