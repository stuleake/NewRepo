using System;

namespace TQ.Core.Filters
{
    /// <summary>
    /// Swagger Ignore Attribute
    /// </summary>
    [AttributeUsageAttribute(AttributeTargets.All)]
    public sealed class SwaggerIgnoreAttribute : Attribute
    {
    }
}