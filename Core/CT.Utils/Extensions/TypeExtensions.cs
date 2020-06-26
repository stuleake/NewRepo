using System;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// Empty constructor delegate of a particular type
    /// </summary>
    /// <param name="type">The type of expected delegate</param>
    /// <returns>Empty constructor delegate</returns>
    public delegate EmptyCtorDelegate EmptyCtorFactoryDelegate(Type type);

    /// <summary>
    /// Empty constructor delegate
    /// </summary>
    /// <returns>Object</returns>
    public delegate object EmptyCtorDelegate();

    /// <summary>
    /// Class to handle Type extensions
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Extract the type code of the requested Type
        /// </summary>
        /// <param name="type">The source Type</param>
        /// <returns>Returns the TypeCode, which specifies the type of an object</returns>
        public static TypeCode GetTypeCode(this Type type)
        {
            return Type.GetTypeCode(type);
        }

        /// <summary>
        /// Ckeck if the instance is of the same type or its parent
        /// </summary>
        /// <param name="type">The source type</param>
        /// <param name="thisOrBaseType">The base type for the type</param>
        /// <returns>Returns true if the instance is of same type and false if it is of parent type</returns>
        public static bool IsInstanceOf(this Type type, Type thisOrBaseType)
        {
            while (type != null)
            {
                if (type == thisOrBaseType)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Check if the type is generic or not
        /// </summary>
        /// <param name="type">The source Type</param>
        /// <returns>Returns true if the type is generic</returns>
        public static bool HasGenericType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType)
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Get the first underlaying generic Type
        /// </summary>
        /// <param name="type">The source Type</param>
        /// <returns>Returns the first underlying generic type if found, returns null otherwise</returns>
        public static Type FirstGenericType(this Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType)
                {
                    return type;
                }

                type = type.BaseType;
            }

            return null;
        }

        /// <summary>
        /// Get the first type that matches with generic definition of a specific type
        /// </summary>
        /// <param name="type">Source type</param>
        /// <param name="genericTypeDefinition">Generic defination of the type to match</param>
        /// <returns>Returns the first type that matches with the generic defination of the specified type, else returns null if there is no match</returns>
        public static Type GetTypeWithGenericTypeDefinitionOf(this Type type, Type genericTypeDefinition)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            foreach (var typ in type.GetInterfaces())
            {
                if (typ.IsGenericType && typ.GetGenericTypeDefinition() == genericTypeDefinition)
                {
                    return typ;
                }
            }

            var genericType = type.FirstGenericType();
            if (genericType != null && genericType.GetGenericTypeDefinition() == genericTypeDefinition)
            {
                return genericType;
            }

            return null;
        }

        /// <summary>
        /// Get the type that matches with any of generic definition type
        /// </summary>
        /// <param name="type">The source Type</param>
        /// <param name="genericTypeDefinitions">Generic type definations to match</param>
        /// <returns>Returns the type that matches with any of the specified generic defination, else returns null if there is no match</returns>
        public static Type GetTypeWithGenericTypeDefinitionOfAny(this Type type, params Type[] genericTypeDefinitions)
        {
            if (genericTypeDefinitions == null)
            {
                throw new ArgumentNullException(nameof(genericTypeDefinitions));
            }
            foreach (var genericTypeDefinition in genericTypeDefinitions)
            {
                var genericType = type.GetTypeWithGenericTypeDefinitionOf(genericTypeDefinition);
                if (genericType == null && type == genericTypeDefinition)
                {
                    genericType = type;
                }

                if (genericType != null)
                {
                    return genericType;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the the type is a generic implementation of another type or interface
        /// </summary>
        /// <param name="type">Source type</param>
        /// <param name="genericTypeDefinition">Generic type defination to match</param>
        /// <returns>Returns true if the type is a generic implementation of another type</returns>
        public static bool IsOrHasGenericInterfaceTypeOf(this Type type, Type genericTypeDefinition)
        {
            return (type.GetTypeWithGenericTypeDefinitionOf(genericTypeDefinition) != null)
                || (type == genericTypeDefinition);
        }

        /// <summary>
        /// Get the underlaying type that implements the desired interface
        /// </summary>
        /// <param name="type">Source type</param>
        /// <param name="interfaceType">Generic type defination to match</param>
        /// <returns>Returns the underlaying type that implements the desired interface if there is a match, returns null otherwise</returns>
        public static Type GetTypeWithInterfaceOf(this Type type, Type interfaceType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (type == interfaceType)
            {
                return interfaceType;
            }

            foreach (var t in type.GetInterfaces())
            {
                if (t == interfaceType)
                {
                    return t;
                }
            }

            return null;
        }

        /// <summary>
        /// Check if the type is implementation of an interface
        /// </summary>
        /// <param name="type">Type declaration</param>
        /// <param name="interfaceType">Type of the interface</param>
        /// <returns>Returns true if the type is and implementation of an interface</returns>
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            foreach (var t in type.GetInterfaces())
            {
                if (t == interfaceType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if all derived types have implement
        /// </summary>
        /// <param name="assignableFromType">Assignable types to be derived from</param>
        /// <param name="types">Array of types to check</param>
        /// <returns>Returns true if all derived types have implement</returns>
        public static bool AllHaveInterfacesOfType(this Type assignableFromType, params Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            foreach (var type in types)
            {
                if (assignableFromType.GetTypeWithInterfaceOf(type) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the type is nullable or not
        /// </summary>
        /// <param name="type">Type declaration</param>
        /// <returns>Returns true if type is nullable</returns>
        public static bool IsNullableType(this Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            return type.IsGenericType ? type.GetGenericTypeDefinition() == typeof(Nullable<>) : !type.IsValueType;
        }

        /// <summary>
        /// Get the underlaying type code
        /// </summary>
        /// <param name="type">Type declaration</param>
        /// <returns>Returns TypeCode, which specifies the type of an object</returns>
        public static TypeCode GetUnderlyingTypeCode(this Type type)
        {
            return GetTypeCode(Nullable.GetUnderlyingType(type) ?? type);
        }

        /// <summary>
        /// Get the first type that matches the generic interface of the desired type
        /// </summary>
        /// <param name="type">Type declaration</param>
        /// <param name="genericInterfaceType">Generic type</param>
        /// <returns>Returns the first that matches the generic interface of the desired type, returns null if there is no match</returns>
        public static Type GetTypeWithGenericInterfaceOf(this Type type, Type genericInterfaceType)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            foreach (var t in type.GetInterfaces())
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == genericInterfaceType)
                {
                    return t;
                }
            }

            if (!type.IsGenericType)
            {
                return null;
            }

            var genericType = type.FirstGenericType();
            return genericType.GetGenericTypeDefinition() == genericInterfaceType
                    ? genericType
                    : null;
        }

        /// <summary>
        /// Check if any of the types match the generic interface of the desired type
        /// </summary>
        /// <param name="genericType">Generic type declaration</param>
        /// <param name="theseGenericTypes">Array of generic types</param>
        /// <returns>Returns true if any of the types match the generic interface of the desired type</returns>
        public static bool MatchesAnyGenericTypeDefinition(this Type genericType, params Type[] theseGenericTypes)
        {
            if (theseGenericTypes == null)
            {
                throw new ArgumentNullException(nameof(theseGenericTypes));
            }
            if (genericType == null)
            {
                throw new ArgumentNullException(nameof(genericType));
            }
            if (!genericType.IsGenericType)
            {
                return false;
            }

            var genericTypeDefinition = genericType.GetGenericTypeDefinition();

            foreach (var thisGenericType in theseGenericTypes)
            {
                if (genericTypeDefinition == thisGenericType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the generic arguments if both the types share a common generic type
        /// </summary>
        /// <param name="assignableFromType">Assignable type to get generic interface</param>
        /// <param name="typeA">First type to check for common generic type</param>
        /// <param name="typeB">Second type to check for common generic type</param>
        /// <returns>Returns the generic arguments if both the types share a common generic type, otherwise it returns null</returns>
        public static Type[] GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(this Type assignableFromType, Type typeA, Type typeB)
        {
            var typeAInterface = typeA.GetTypeWithGenericInterfaceOf(assignableFromType);
            if (typeAInterface == null)
            {
                return Array.Empty<Type>();
            }

            var typeBInterface = typeB.GetTypeWithGenericInterfaceOf(assignableFromType);
            if (typeBInterface == null)
            {
                return Array.Empty<Type>();
            }

            var typeAGenericArgs = typeAInterface.GetGenericArguments();
            var typeBGenericArgs = typeBInterface.GetGenericArguments();

            if (typeAGenericArgs.Length != typeBGenericArgs.Length)
            {
                return Array.Empty<Type>();
            }

            for (var i = 0; i < typeBGenericArgs.Length; i++)
            {
                if (typeAGenericArgs[i] != typeBGenericArgs[i])
                {
                    return Array.Empty<Type>();
                }
            }

            return typeAGenericArgs;
        }

        /// <summary>
        /// Check if all types are string or value types
        /// </summary>
        /// <param name="types">Array of types to be checked</param>
        /// <returns>Returns true if all the types are string or value type</returns>
        public static bool AreAllStringOrValueTypes(params Type[] types)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }
            foreach (var type in types)
            {
                if (!(type == typeof(string) || type.IsValueType))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Change the type of the object to desired type.
        /// </summary>
        /// <param name="value">Object for which the type has to be changed</param>
        /// <param name="type">The desired type that the object has to be changed to</param>
        /// <returns>Returns an object with its type changed to the desired type</returns>
        public static object ChangeType(this object value, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if ((value == null || value == DBNull.Value) && type.IsGenericType)
            {
                return Activator.CreateInstance(type);
            }

            if (value == null || value == DBNull.Value)
            {
                return null;
            }

            if (type == value.GetType())
            {
                return value;
            }

            if (type.IsEnum)
            {
                return value is string ? Enum.Parse(type, value as string) : Enum.ToObject(type, value);
            }

            if (!type.IsInterface && type.IsGenericType)
            {
                Type innerType = type.GetGenericArguments()[0];
                object innerValue = ChangeType(value, innerType);
                return Activator.CreateInstance(type, new object[] { innerValue });
            }

            if (value is string && type == typeof(Guid))
            {
                if (string.IsNullOrEmpty(value as string))
                {
                    return Guid.Empty;
                }

                return new Guid(value as string);
            }

            if (value is string && type == typeof(Version))
            {
                return new Version(value as string);
            }

            if (!(value is IConvertible))
            {
                return value;
            }

            return Convert.ChangeType(value, type);
        }
    }
}