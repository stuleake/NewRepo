using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Api.Planner
{
    /// <summary>
    /// A class to create and start the WebHost for the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Creates the Web Host Builder to start the application
        /// </summary>
        public static void Main()
        {
            CreateWebHostBuilder().Build().Run();
        }

        /// <summary>
        /// A method to Create Web Host Builder
        /// </summary>
        /// <returns>Returns Web Host Builder</returns>
        public static IWebHostBuilder CreateWebHostBuilder() =>
            WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();
    }
}