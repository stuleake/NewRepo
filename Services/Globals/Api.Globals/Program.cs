using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Api.Globals
{
    /// <summary>
    /// entry point for the code
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main is the starting point for exceution
        /// </summary>
        public static void Main()
        {
            CreateWebHostBuilder().Build().Run();
        }

        /// <summary>
        /// To create and configure host in web apps
        /// </summary>
        /// <returns>Returrns an IWebBuilder</returns>
        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
    }
}