using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using TQ.Core.Exceptions;
using TQ.Core.Models;

namespace TQ.Core.Filters
{
    /// <summary>
    /// A class to filter attributes for custom exceptions Attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomExceptionFilterAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Updating exception context result for TQException(custom Exception)
        /// </summary>
        /// <param name="context">object of ExceptionContext</param>
        public override void OnException(ExceptionContext context)
        {
            if (context?.Exception is TQException)
            {
                var res = context.Exception.Message;
                var response = new BadResultModel { Success = false, Message = res };
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(response);
            }
        }
    }
}