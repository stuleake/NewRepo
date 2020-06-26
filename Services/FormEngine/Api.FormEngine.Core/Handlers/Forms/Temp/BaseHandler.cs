using Api.FormEngine.Core.Commands;
using CT.KeyVault;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using TQ.Data.PlanningPortal;

namespace Api.FormEngine.Core.Handlers
{
    public class BaseHandler
    {
        private readonly IVaultManager keyvault;

        private readonly IConfiguration config;

        public BaseHandler(IVaultManager keyvault, IConfiguration config)
        {
            this.keyvault = keyvault;
            this.config = config;
        }

        public static string GetCountry(BaseCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            return (cmd.Country ?? string.Empty).ToUpper();
        }

        public async Task<PlanningPortalContext> GetContextAsync(BaseCommand cmd)
        {
            string country = GetCountry(cmd);
            string connection = string.Empty;
            switch (country)
            {
                case "ENGLAND":
                    connection = keyvault.GetSecret(config["SqlConnection:England-PlanningPortal"]);
                    break;

                case "WALES":
                    connection = keyvault.GetSecret(config["SqlConnection:Wales-PlanningPortal"]);
                    break;
            }

            var opt = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<PlanningPortalContext>(), connection).Options;
            PlanningPortalContext planningPortalContext = new PlanningPortalContext(opt);

            return await Task.FromResult(planningPortalContext).ConfigureAwait(false);
        }
    }
}