using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// class to handle reflection Extensions
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Get the module of the Type
        /// </summary>
        /// <param name="type">Type to get the the module from</param>
        /// <returns>Returns the module of the required type if type is not null, returns null otherwise</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Module GetModule(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            return type.Module;
        }

        /// <summary>
        /// Get all properties of the particular object type
        /// </summary>
        /// <param name="type">Type to get the the properties from</param>
        /// <returns>Returns the properties of the specified object type</returns>
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface))
                        {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(t => t.GetIndexParameters().Length == 0) // ignore indexed properties
                .ToArray();
        }

        /// <summary>
        /// Get public properties of the particular object type
        /// </summary>
        /// <param name="type">Type to get the the public properties from</param>
        /// <returns> Returns the public properties of the specefied object type</returns>
        public static PropertyInfo[] GetPublicProperties(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (type.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface))
                        {
                            continue;
                        }

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance)
                .Where(t => t.GetIndexParameters().Length == 0) // ignore indexed properties
                .ToArray();
        }

        /// <summary>
        /// Get the particular attribute based on the property name
        /// </summary>
        /// <typeparam name="T">The Type to get attributes from</typeparam>
        /// <param name="instance">The object instance to get the attributes from</param>
        /// <param name="propertyName">Property name for the attributes to be take from</param>
        /// <returns>Returns the particular attribute based on the property name</returns>
        public static T GetAttributeFrom<T>(this object instance, string propertyName)
            where T : Attribute
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).First();
        }

        /// <summary>
        /// Copy properties from source to destination, both source and destination objects should be same
        /// </summary>
        /// <typeparam name="TSource">The source Type</typeparam>
        /// <typeparam name="TDestination">The destination Type</typeparam>
        /// <param name="source">The source type</param>
        /// <param name="destination">The destination type</param>
        /// <param name="copyNullValues">Specifies if the null values have to be copied</param>
        public static void CopyPropertiesTo<TSource, TDestination>(this TSource source, TDestination destination, bool copyNullValues = false)
            where TSource : class
            where TDestination : class
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var sourceProps = source.GetType().GetPublicProperties().Where(x => x.CanRead).ToList();
            var destProps = destination.GetType().GetPublicProperties().Where(x => x.CanWrite).ToList();
            foreach (var sourceProp in sourceProps)
            {
                if (destProps.Any(x => x.Name == sourceProp.Name))
                {
                    var p = destProps.First(x => x.Name == sourceProp.Name);
                    var sourceValue = sourceProp.GetValue(source, null);
                    var destValue = p.GetValue(destination, null);
                    if (p.CanWrite)
                    {
                        if (copyNullValues)
                        {
                            p.SetValue(destination, sourceProp.GetValue(source, null), null);
                        }
                        else if (sourceValue?.Equals(destValue) == false)
                        {
                            // check if the property can be set or no
                            p.SetValue(destination, sourceProp.GetValue(source, null), null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates an instance of an object based on the class name and assembly name
        /// </summary>
        /// <typeparam name="T">Generic type defination</typeparam>
        /// <param name="className">Class name from which the instance has to be created</param>
        /// <param name="assemblyName">Assembly name from which the instance has to be created</param>
        /// <param name="nameSpace">Namespace from which the instance has to be created</param>
        /// <param name="ignoreCase">Specifiesif the case has to be ignored</param>
        /// <returns>Returns the instance of an object based on the class name and assembly name</returns>
        public static T CreateObject<T>(this string className, string assemblyName, string nameSpace = "", bool ignoreCase = true)
        {
            Assembly assembly = Assembly.Load(assemblyName);

            string actualClassName = $"{assemblyName}.{className}";

            if (!string.IsNullOrEmpty(nameSpace))
            {
                actualClassName = $"{assemblyName}.{nameSpace}.{className}";
            }

            return (T)assembly.CreateInstance(actualClassName, ignoreCase);
        }

        /// <summary>
        /// Gets the type of object based on class name, assembly name and namespace
        /// </summary>
        /// <param name="className">Class name to get the type of object</param>
        /// <param name="assemblyName">Assembly name to get the type of object</param>
        /// <param name="nameSpace">Namespace to get the type of object</param>
        /// <returns>Returns the type of object based on class name, assembly name and namespace</returns>
        public static Type GetType(this string className, string assemblyName, string nameSpace = "")
        {
            if (string.IsNullOrEmpty(className))
            {
                throw new ArgumentNullException(nameof(className));
            }

            Assembly assembly = Assembly.Load(assemblyName);
            var actualclassName = $"{assemblyName}.{className}";                 // SUGGESTION1: Let the user to send with case sensitive data

            if (!string.IsNullOrEmpty(nameSpace))
            {
                nameSpace = $"{assemblyName}.{nameSpace}";
                var typeMap = assembly.GetTypes()
                         .Where(t => t.Namespace == nameSpace)
                         .ToDictionary(t => t.Name.ToLower(), t => t, StringComparer.OrdinalIgnoreCase);
                if (typeMap.TryGetValue(className.ToLower(), out Type type))
                {
                    return type;
                }
            }

            return assembly.GetType(actualclassName);
        }

        /// <summary>
        /// assigns the properties of the given object from the keyvalue pairs
        /// Key names should be same as the object property name
        /// </summary>
        /// <param name="instance">object instance whose properties have to be assigned</param>
        /// <param name="properties">list of key value pair of properties</param>
        public static void SetObjectPropertiesFromKeyValuePair(this object instance, IEnumerable<KeyValuePair<string, object>> properties)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            var type = instance.GetType();
            foreach (var property in type.GetPublicProperties())
            {
                var propertyType = property.PropertyType;
                if (property.CanWrite && properties.Any(k => k.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    property.SetValue(instance, properties.FirstOrDefault(k => k.Key.Equals(property.Name, StringComparison.InvariantCultureIgnoreCase)).Value.ChangeType(propertyType));
                }
            }
        }

        /// <summary>
        /// USE WITH CAUTION : Invokes generic type of methods using reflection
        /// </summary>
        /// <param name="classObjectInstance">class where the method is available</param>
        /// <param name="genericType">Generic type</param>
        /// <param name="methodNameToCall">name of the method</param>
        /// <param name="parameters">parameters to be passed</param>
        /// <returns>Returns an invoked generic method</returns>
        public static object InvokeGenericMethod(
            this object classObjectInstance,
            Type genericType,
            string methodNameToCall,
            object[] parameters = null)
        {
            if (classObjectInstance == null)
            {
                throw new ArgumentNullException(nameof(classObjectInstance));
            }
            var classObjectType = classObjectInstance.GetType();
            var method = Array.Find(classObjectType.GetMethods(), e => e.Name.Equals(methodNameToCall, StringComparison.InvariantCultureIgnoreCase));
            var genericMethod = method.MakeGenericMethod(genericType);
            return genericMethod.Invoke(classObjectInstance, parameters);
        }

        /// <summary>
        /// Invokes generic type of methods using reflection
        /// </summary>
        /// <param name="classObjectInstance">class in which the method is available</param>
        /// <param name="genericTYpe">generic type.</param>
        /// <param name="methodNameToCall">name of the method</param>
        /// <param name="parameters">parameters to be passed</param>
        /// <returns>Returns an invoked generic method</returns>
        public static object InvokeGenericMethod(
            this object classObjectInstance,
            Type[] genericTYpe,
            string methodNameToCall,
            params object[] parameters)
        {
            if (classObjectInstance == null)
            {
                throw new ArgumentNullException(nameof(classObjectInstance));
            }
            var classObjectType = classObjectInstance.GetType();
            var method = Array.Find(classObjectType.GetMethods(), emptyCtorDelegate => emptyCtorDelegate.Name.Equals(methodNameToCall, StringComparison.InvariantCultureIgnoreCase));
            var genericmethod = method.MakeGenericMethod(genericTYpe);
            return genericmethod.Invoke(classObjectInstance, parameters);
        }

        /// <summary>
        /// Set property of an object if that property exists
        /// </summary>
        /// <param name="obj">The object to set property for</param>
        /// <param name="property">Propetty to set</param>
        /// <param name="value">Value to be set</param>
        public static void TrySetProperty(this object obj, string property, object value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            var prop = obj.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
            if (prop?.CanWrite == true)
            {
                prop.SetValue(obj, value, null);
            }
        }
    }
}