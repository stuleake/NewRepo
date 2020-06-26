using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TQ.Core.Helpers
{
    /// <summary>
    /// The HealthCheck helper class
    /// </summary>
    public static class HealthCheck
    {
        /// <summary>
        /// The SqlHealthcheckQuery to execute
        /// </summary>
        public static readonly string SqlHealthcheckQuery = "SELECT 1;";

        /// <summary>
        /// The AzureKeyVaultName Health-check name
        /// </summary>
        public static readonly string AzureKeyVaultName = "Azure-Keyvault";

        /// <summary>
        /// The AzureStorageName Health-check name
        /// </summary>
        public static readonly string AzureStorageName = "Azure-Storage";

        /// <summary>
        /// The FormsEngineSqlServerName Health-check name
        /// </summary>
        public static readonly string FormsEngineSqlServerName = "Forms-Engine-SQL-Server";

        /// <summary>
        /// The WelshPlanningPortalSqlServerName Health-check name
        /// </summary>
        public static readonly string WelshPlanningPortalSqlServerName = "Welsh-Planning-Portal-SQL-Server";

        /// <summary>
        /// The EnglishPlanningPortalSqlServerName Health-check name
        /// </summary>
        public static readonly string EnglishPlanningPortalSqlServerName = "English-Planning-Portal-SQL-Server";

        /// <summary>
        /// The custom health check response writer
        /// </summary>
        /// <param name="context">the HTTP context to use</param>
        /// <param name="healthReport">the health report to use</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static Task CustomHealthCheckResponseWriterAsync(HttpContext context, HealthReport healthReport)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (healthReport == null)
            {
                throw new ArgumentException(nameof(context));
            }

            context.Response.ContentType = "application/json";

            var result = JsonConvert.SerializeObject(new
            {
                status = healthReport.Status.ToString(),
                entries = healthReport.Entries.Select(e => new
                {
                    key = e.Key,
                    value = e.Value.Status.ToString()
                }),
            });

            return context.Response.WriteAsync(result);
        }
    }
}