using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TQ.Core.Extensions
{
    /// <summary>
    /// Enum Extension
    /// </summary>
    /// <typeparam name="T">Type Parameter</typeparam>
    public static class EnumExtensions<T>
        where T : struct, IConvertible
    {
        /// <summary>
        /// Get enum value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>return enum value</returns>
        public static IList<T> GetValues(Enum value)
        {
            var enumValues = new List<T>();

            foreach (FieldInfo fi in value?.GetType().GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumValues.Add((T)Enum.Parse(value?.GetType(), fi.Name, false));
            }
            return enumValues;
        }

        /// <summary>
        /// Parse enum value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>return parsed enum value</returns>
        public static T Parse(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Parse enum value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <param name="ignoreCase">set ignore case</param>
        /// <returns>return parsed display name</returns>
        public static T ParseDisplayName(string value, bool ignoreCase = true)
        {
            List<EnumResults> enumMetadata = GetEnumMetadata();

            EnumResults currentEnumMetada = ignoreCase
                ? enumMetadata.FirstOrDefault(x => string.Compare(x.DisplayAttribute.Name, value, StringComparison.InvariantCultureIgnoreCase) == 0)
                : enumMetadata.FirstOrDefault(x => string.Compare(x.DisplayAttribute.Name, value, StringComparison.InvariantCulture) == 0);

            if (currentEnumMetada != null && Enum.TryParse<T>(currentEnumMetada.FieldInfo.Name, out T parsedEnum))
            {
                return parsedEnum;
            }
            else
            {
                throw new ArgumentNullException(value);
            }
        }

        /// <summary>
        /// Get names of enum
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>return name of enum value</returns>
        public static IList<string> GetNames(Enum value)
        {
            return value?.GetType().GetFields(BindingFlags.Static | BindingFlags.Public).Select(fi => fi.Name).ToList();
        }

        /// <summary>
        /// Get display value
        /// </summary>
        /// <param name="value">enum value</param>
        /// <returns>return display value</returns>
        public static IList<string> GetDisplayValues(Enum value)
        {
            return GetNames(value).Select(obj => GetDisplayValue(Parse(obj))).ToList();
        }

        /// <summary>
        /// Get display value
        /// </summary>
        /// <param name="value">value parameter</param>
        /// <returns>return display value</returns>
        public static string GetDisplayValue(T value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(DisplayAttribute), false) as DisplayAttribute[];

            if (descriptionAttributes[0].ResourceType != null)
            {
                return LookupResource(descriptionAttributes[0].ResourceType, descriptionAttributes[0].Name);
            }

            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        private static string LookupResource(Type resourceManagerProvider, string resourceKey)
        {
            foreach (PropertyInfo staticProperty in resourceManagerProvider.GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (staticProperty.PropertyType == typeof(System.Resources.ResourceManager))
                {
                    System.Resources.ResourceManager resourceManager = (System.Resources.ResourceManager)staticProperty.GetValue(null, null);
                    return resourceManager.GetString(resourceKey);
                }
            }

            return resourceKey; // Fallback with the key name
        }

        private static List<EnumResults> GetEnumMetadata()
        {
            var fieldsInfo = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public).ToList();
            var enumMetadata = new List<EnumResults>();
            foreach (var fieldInfo in fieldsInfo)
            {
                var descriptionAttribute = fieldInfo.GetCustomAttribute<DisplayAttribute>();
                if (descriptionAttribute != null)
                {
                    var enumResult = new EnumResults
                    {
                        DisplayAttribute = descriptionAttribute,
                        FieldInfo = fieldInfo
                    };
                    enumMetadata.Add(enumResult);
                }
            }

            return enumMetadata;
        }
    }
}