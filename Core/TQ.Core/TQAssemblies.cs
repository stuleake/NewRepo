using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TQ.Core
{
    /// <summary>
    /// Assemblies associated with TQ
    /// </summary>
    public static class TQAssemblies
    {
        /// <summary>
        /// Gets all TQ Assemblies
        /// </summary>
        /// <returns>returns a list of target assemblies</returns>
        public static IEnumerable<Assembly> AllAssemblies
        {
            get
            {
                var targetAssemblies = new[] { "Api.", "TQ.", "CT." };
                return FindAssemblies(targetAssemblies);
            }
        }

        /// <summary>
        /// Gets all TQ Api Assemblies
        /// </summary>
        /// <returns>returns a list of target assemblies</returns>
        public static IEnumerable<Assembly> ApiAssemblies
        {
            get
            {
                var targetAssemblies = new[] { "Api." };
                return FindAssemblies(targetAssemblies);
            }
        }

        private static IEnumerable<Assembly> FindAssemblies(IEnumerable<string> targetAssemblies)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(assembly => targetAssemblies.Any(target => assembly.GetName().Name.StartsWith(target, StringComparison.InvariantCulture)));
        }
    }
}