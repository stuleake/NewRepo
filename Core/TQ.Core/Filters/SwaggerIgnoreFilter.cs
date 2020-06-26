using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Reflection;

namespace TQ.Core.Filters
{
    /// <summary>
    /// Swagger Ignore filter
    /// </summary>
    public class SwaggerIgnoreFilter : IDocumentFilter
    {
        /// <summary>
        /// Method for Swagger document
        /// </summary>
        /// <param name="swaggerDoc">object of swagger Document</param>
        /// <param name="context">Document Filter Context</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var allTypes = TQAssemblies.ApiAssemblies?.SelectMany(i => i.GetTypes())?.AsEnumerable();

            foreach (var definition in swaggerDoc?.Components.Schemas)
            {
                var type = allTypes?.FirstOrDefault(x => x.Name == definition.Key);
                if (type != null)
                {
                    var properties = type.GetProperties();
                    foreach (var prop in properties.ToList())
                    {
                        var ignoreAttribute = prop.GetCustomAttribute(typeof(SwaggerIgnoreAttribute), false);

                        if (ignoreAttribute != null)
                        {
                            definition.Value.Properties.Remove(ConvertFirstLetterToSmallCase(prop.Name));
                        }
                    }
                }
            }
        }

        private static string ConvertFirstLetterToSmallCase(string input)
        {
            return input.Substring(0, 1).ToLower() + input.Substring(1);
        }
    }
}