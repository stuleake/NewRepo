using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Api.FormEngine
{
    /// <summary>
    /// A class to create and start the WebHost for the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Creates the Web Host Builder to start the application
        /// </summary>
        /// <param name="args">Arguments to start the application</param>
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// A method to Create Web Host Builder
        /// </summary>
        /// <param name="args">Arguments to build hte host</param>
        /// <returns>Returns Web Host Builder</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}