using CT.Utils.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static UnitTest.CT.Utils.Extensions.IntegerExtensionsTest;
using static UnitTest.CT.Utils.Extensions.StringExtensionsTest;

namespace UnitTest.CT.Utils.Extensions
{
    /// <summary>
    /// Type extensions
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TypeExtensionsTest
    {
        /// <summary>
        /// Private interface ITest
        /// </summary>
        private interface ITest
        {
        }

        /// <summary>
        /// Private interface ITestGeneric
        /// </summary>
        /// <typeparam name="T">Generic type for the interface</typeparam>
        private interface ITestGeneric<T>
        {
        }

        private class TestSuperClass : ITest
        {
        }

        private class TestSubClass : TestSuperClass
        {
        }

        /// <summary>
        /// Internal class TestClass
        /// </summary>
        internal class TestClass
        {
        }

        private class TestGenericClass<T> : ITestGeneric<T>
        {
        }

        private class TestGenericSubClass : TestGenericClass<TestClass>
        {
        }

        private class TestTypeA<T> : ITestGeneric<T>
        {
        }

        private class TestTypeB<T> : ITestGeneric<T>
        {
        }

        /// <summary>
        /// Test to get type code
        /// </summary>
        [Fact]
        public void GetTypeCodeTest()
        {
            // Arrange
            var inttype = typeof(int);
            var objecttype = typeof(TestClass);

            // Act
            var inttyperesult = inttype.GetTypeCode();
            var objecttyperesult = objecttype.GetTypeCode();

            // Assert
            Assert.Equal(TypeCode.Int32, inttyperesult);
            Assert.Equal(TypeCode.Object, objecttyperesult);
        }

        /// <summary>
        /// Test to get instance
        /// </summary>
        [Fact]
        public void IsInstanceOfTest()
        {
            // Arrange
            var @type = typeof(TestSubClass);

            // Act
            var falseresult = type.IsInstanceOf(typeof(ITest));
            var trueresult = type.IsInstanceOf(typeof(TestSubClass));

            // Assert
            Assert.False(falseresult);
            Assert.True(trueresult);
        }

        /// <summary>
        /// Test to check generic type
        /// </summary>
        [Fact]
        public void HasGenericTypeTest()
        {
            // Arrange
            var @generictype = typeof(TestGenericClass<TestClass>);
            var @basegenerictype = typeof(TestGenericSubClass);
            var @nongenerictype = typeof(TestClass);

            // Act
            var genericresult = @generictype.HasGenericType();
            var basegenericresult = @basegenerictype.HasGenericType();
            var nongenericresult = @nongenerictype.HasGenericType();

            // Assert
            Assert.True(genericresult);
            Assert.True(basegenericresult);
            Assert.False(nongenericresult);
        }

        /// <summary>
        /// Test to check first generic type
        /// </summary>
        [Fact]
        public void FirstGenericTypeTest()
        {
            // Arrange
            var @basegenerictype = typeof(TestGenericSubClass);
            var @itselfgenerictype = typeof(TestGenericClass<>);

            // Act
            var basegenerictyperesult = @basegenerictype.FirstGenericType();
            var itselfgenerictyperesult = itselfgenerictype.FirstGenericType();

            // Assert
            Assert.Equal(typeof(TestGenericClass<TestClass>), basegenerictyperesult);
            Assert.Equal(typeof(TestGenericClass<>), itselfgenerictyperesult);
        }

        /// <summary>
        /// Unit test for GetTypeWithGenericTypeDefinitionOfTest
        /// </summary>
        [Fact]
        public void GetTypeWithGenericTypeDefinitionOfTest()
        {
            // Arrange
            var type = typeof(TestGenericSubClass);

            // Act
            var basegenericresult = type.GetTypeWithGenericTypeDefinitionOf(typeof(TestGenericClass<>));
            var interfacegenericresult = type.GetTypeWithGenericTypeDefinitionOf(typeof(ITestGeneric<>));

            // Assert
            Assert.Equal(typeof(TestGenericClass<TestClass>), basegenericresult);
            Assert.Equal(typeof(ITestGeneric<TestClass>), interfacegenericresult);
        }

        /// <summary>
        /// Unit test for GetTypeWithGenericTypeDefinitionOfAnyTest
        /// </summary>
        [Fact]
        public void GetTypeWithGenericTypeDefinitionOfAnyTest()
        {
            // Arrange
            var type = typeof(TestGenericSubClass);
            var type2 = typeof(TestClass);

            // Act
            var result = type.GetTypeWithGenericTypeDefinitionOfAny(typeof(TestClass), typeof(TestGenericClass<>));
            var result2 = type2.GetTypeWithGenericTypeDefinitionOfAny(null, typeof(TestGenericClass<>));
            var result3 = type2.GetTypeWithGenericTypeDefinitionOfAny(null, typeof(TestClass));

            // Assert
            Assert.Equal(typeof(TestGenericClass<TestClass>), result);
            Assert.Null(result2);
            Assert.Equal(typeof(TestClass), result3);
        }

        /// <summary>
        /// Unit test for IsOrHasGenericInterfaceTypeOfTest
        /// </summary>
        [Fact]
        public void IsOrHasGenericInterfaceTypeOfTest()
        {
            // Arrange
            var generictype = typeof(TestGenericSubClass);
            var nongenerictype = typeof(TestClass);

            // Act
            var genericresult = generictype.IsOrHasGenericInterfaceTypeOf(typeof(ITestGeneric<>));
            var nongenericresult = nongenerictype.IsOrHasGenericInterfaceTypeOf(typeof(ITestGeneric<>));

            // Assert
            Assert.True(genericresult);
            Assert.False(nongenericresult);
        }

        /// <summary>
        /// Unit test for GetTypeWithInterfaceOfTest
        /// </summary>
        [Fact]
        public void GetTypeWithInterfaceOfTest()
        {
            // Arrange
            var typeofsubclassofinterface = typeof(TestSubClass);
            var typeofnonsubclassofinterface = typeof(TestClass);
            var interfacetype = typeof(ITest);

            // Act
            var subclasstypeofinterfaceresult = typeofsubclassofinterface.GetTypeWithInterfaceOf(typeof(ITest));
            var nonsubclasstypeofinterfaceresult = typeofnonsubclassofinterface.GetTypeWithInterfaceOf(typeof(ITest));
            var interfacetyperesult = interfacetype.GetTypeWithInterfaceOf(typeof(ITest));

            // Assert
            Assert.NotNull(typeofsubclassofinterface);
            Assert.Null(nonsubclasstypeofinterfaceresult);
            Assert.NotNull(interfacetyperesult);
            Assert.Equal(typeof(ITest), interfacetyperesult);
            Assert.Equal(typeof(ITest), subclasstypeofinterfaceresult);
        }

        /// <summary>
        /// Unit test for HasInterfaceTest
        /// </summary>
        [Fact]
        public void HasInterfaceTest()
        {
            // Arrange
            var typeofsubclassofinterface = typeof(TestSubClass);
            var typeofnonsubclassofinterface = typeof(TestClass);

            // Act
            var subclassofinterfaceresult = typeofsubclassofinterface.HasInterface(typeof(ITest));
            var nonsubclassofinterfaceresult = typeofnonsubclassofinterface.HasInterface(typeof(ITest));

            // Assert
            Assert.True(subclassofinterfaceresult);
            Assert.False(nonsubclassofinterfaceresult);
        }

        /// <summary>
        /// Unit test for AllHaveInterfacesOfTypeTest
        /// </summary>
        [Fact]
        public void AllHaveInterfacesOfTypeTest()
        {
            // Arrange
            var assignabletype = typeof(TestSubClass);

            // Act
            var trueresult = assignabletype.AllHaveInterfacesOfType(typeof(ITest));
            var falseresult = assignabletype.AllHaveInterfacesOfType(typeof(ITest), typeof(ITestGeneric<>));

            // Assert
            Assert.True(trueresult);
            Assert.False(falseresult);
        }

        /// <summary>
        /// Unit test for IsNullableTypeTest
        /// </summary>
        [Fact]
        public void IsNullableTypeTest()
        {
            // Arrange
            var nullabletype = typeof(int?);
            var nonnullabletype = typeof(bool);
            var stringtype = typeof(string);
            var classtype = typeof(TestClass);
            var enumtype = typeof(Test);

            // Act
            var nullabletyperesult = nullabletype.IsNullableType();
            var nonnullabletyperesult = nonnullabletype.IsNullableType();
            var stringtyperesult = stringtype.IsNullableType();
            var classtyperesult = classtype.IsNullableType();
            var enumtyperesult = enumtype.IsNullableType();

            // Assert
            Assert.True(nullabletyperesult);
            Assert.False(nonnullabletyperesult);
            Assert.True(stringtyperesult);
            Assert.True(classtyperesult);
            Assert.False(enumtyperesult);
        }

        /// <summary>
        /// Unit test for GetUnderlyingTypeCodeTest
        /// </summary>
        [Fact]
        public void GetUnderlyingTypeCodeTest()
        {
            // Arrange
            var inttype = typeof(int);
            var nullableintype = typeof(int?);
            var classtype = typeof(TestClass);

            // Act
            var intresult = inttype.GetUnderlyingTypeCode();
            var nullableintresult = nullableintype.GetUnderlyingTypeCode();
            var classtyperesult = classtype.GetUnderlyingTypeCode();

            // Assert
            Assert.Equal(TypeCode.Int32, intresult);
            Assert.Equal(TypeCode.Int32, nullableintresult);
            Assert.Equal(TypeCode.Object, classtyperesult);
        }

        /// <summary>
        /// Unit test for GetTypeWithGenericInterfaceOfTest
        /// </summary>
        [Fact]
        public void GetTypeWithGenericInterfaceOfTest()
        {
            // Arrange
            var generictype = typeof(TestGenericClass<TestClass>);

            // Act
            var generictyperesult = generictype.GetTypeWithGenericInterfaceOf(typeof(ITestGeneric<>));

            // Assert
            Assert.NotNull(generictyperesult);
            Assert.Equal(typeof(ITestGeneric<TestClass>), generictyperesult);
        }

        /// <summary>
        /// Unit test for GetTypeWithNonGenericInterfaceOfTest
        /// </summary>
        [Fact]
        public void GetTypeWithNonGenericInterfaceOfTest()
        {
            // Arrange
            var generictype = typeof(TestClass);

            // Act
            var generictyperesult = generictype.GetTypeWithGenericInterfaceOf(typeof(ITestGeneric<>));

            // Assert
            Assert.Null(generictyperesult);
        }

        /// <summary>
        /// Unit test for GetTypeWithGenericTypeDefinitionOfNullCheck
        /// </summary>
        [Fact]
        public void GetTypeWithGenericTypeDefinitionOfNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.GetTypeWithGenericTypeDefinitionOf(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetTypeWithGenericTypeDefinitionOfAnyNullCheck
        /// </summary>
        [Fact]
        public void GetTypeWithGenericTypeDefinitionOfAnyNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.GetTypeWithGenericTypeDefinitionOfAny(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetTypeWithInterfaceOfNullCheck
        /// </summary>
        [Fact]
        public void GetTypeWithInterfaceOfNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.GetTypeWithInterfaceOf(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for HasInterfaceNullCheck
        /// </summary>
        [Fact]
        public void HasInterfaceNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.HasInterface(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for AllHaveInterfacesOfTypeNullCheck
        /// </summary>
        [Fact]
        public void AllHaveInterfacesOfTypeNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.AllHaveInterfacesOfType(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for IsNullableTypeNullCheck
        /// </summary>
        [Fact]
        public void IsNullableTypeNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.IsNullableType();
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for GetTypeWithGenericInterfaceOfNullCheck
        /// </summary>
        [Fact]
        public void GetTypeWithGenericInterfaceOfNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.GetTypeWithGenericInterfaceOf(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for MatchesAnyGenericTypeDefinitionNullCheck
        /// </summary>
        [Fact]
        public void MatchesAnyGenericTypeDefinitionNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.MatchesAnyGenericTypeDefinition();
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);

            Exception error1 = null;

            try
            {
                _ = type.MatchesAnyGenericTypeDefinition(null);
            }
            catch (ArgumentNullException ex)
            {
                error1 = ex;
            }

            // Assert
            Assert.NotNull(error1);
        }

        /// <summary>
        /// Unit test for AreAllStringOrValueTypesNullCheck
        /// </summary>
        [Fact]
        public void AreAllStringOrValueTypesNullCheck()
        {
            // Act
            ArgumentNullException error = null;
            try
            {
                TypeExtensions.AreAllStringOrValueTypes(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for ChangeTypeNullCheck
        /// </summary>
        [Fact]
        public void ChangeTypeNullCheck()
        {
            // Arrange
            Type type = null;

            // Act
            ArgumentNullException error = null;
            try
            {
                _ = type.ChangeType(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Unit test for MatchesAnyGenericTypeDefinitionTest
        /// </summary>
        [Fact]
        public void MatchesAnyGenericTypeDefinitionTest()
        {
            // Arrange
            var generictype = typeof(TestGenericClass<TestClass>);
            var nongenerictype = typeof(TestClass);

            // Act
            var trueresult = generictype.MatchesAnyGenericTypeDefinition(typeof(TestGenericClass<>));
            var falseresult = generictype.MatchesAnyGenericTypeDefinition(typeof(ITestGeneric<>));
            var nongenerictyperesult = nongenerictype.MatchesAnyGenericTypeDefinition(typeof(ITestGeneric<>), typeof(TestGenericClass<>), typeof(ITest));

            // Assert
            Assert.True(trueresult);
            Assert.False(falseresult);
            Assert.False(nongenerictyperesult);
        }

        /// <summary>
        /// Unit test for GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArgumentsTest
        /// </summary>
        [Fact]
        public void GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArgumentsTest()
        {
            // Arrange
            var type = typeof(ITestGeneric<>);
            var emptyArray = Array.Empty<Type>();

            // Act
            var result = type.GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(typeof(TestTypeA<TestClass>), typeof(TestTypeB<TestClass>));
            var nullTypeInterfaceA = type.GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(typeof(TestClass), typeof(TestTypeB<TestClass>));
            var nullTypeInterfaceB = type.GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(typeof(TestTypeA<TestClass>), typeof(TestClass));
            var sameArgLength = type.GetGenericArgumentsIfBothHaveSameGenericDefinitionTypeAndArguments(typeof(TestTypeA<TestClass>), typeof(TestTypeB<>));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(typeof(TestClass), result[0]);
            Assert.Equal(emptyArray, nullTypeInterfaceA);
            Assert.Equal(emptyArray, nullTypeInterfaceB);
            Assert.Equal(emptyArray, sameArgLength);
        }

        /// <summary>
        /// Unit test for AreAllStringOrValueTypesTest
        /// </summary>
        [Fact]
        public void AreAllStringOrValueTypesTest()
        {
            // Arrange
            var stringorvaluetypes = new Type[] { typeof(string), typeof(int), typeof(int?), typeof(bool) };
            var nonstringorvaluetypes = new Type[] { typeof(TestClass), typeof(object), typeof(ITest) };
            var mixedtypes = new Type[] { typeof(string), typeof(object), typeof(int) };

            // Act
            var trueresult = TypeExtensions.AreAllStringOrValueTypes(stringorvaluetypes);
            var falseresult = TypeExtensions.AreAllStringOrValueTypes(nonstringorvaluetypes);
            var mixedtyperesult = TypeExtensions.AreAllStringOrValueTypes(mixedtypes);

            // Assert
            Assert.True(trueresult);
            Assert.False(falseresult);
            Assert.False(mixedtyperesult);
        }

        /// <summary>
        /// Unit test for ChangeTypeTest
        /// </summary>
        [Fact]
        public void ChangeTypeTest()
        {
            // Arrange
            object sourcetype = "2d4a02f3-c624-42fa-a39a-455192f8d898";
            object enumstring = "ABC";
            object nullValue = null;
            string emptyString = string.Empty;
            Guid emptyGuid = Guid.Empty;
            string version = "1.0.0.0";
            var person = Tuple.Create("notIConvertable", "test");
            var activatorInstance = Activator.CreateInstance(typeof(TestGenericClass<TestClass>)).ToString();

            // Act
            var createInstanceResult = nullValue.ChangeType(typeof(TestGenericClass<TestClass>));
            var guidresult = sourcetype.ChangeType(typeof(Guid));
            var nullGuidResult = emptyString.ChangeType(typeof(Guid));
            var enumresult = enumstring.ChangeType(typeof(TestString));
            var nullResult = nullValue.ChangeType(typeof(TestClass));
            var versionResult = version.ChangeType(typeof(Version));
            var notIconvertableResult = person.ChangeType(typeof(float));

            // Assert
            Assert.Equal(activatorInstance, createInstanceResult.ToString());
            Assert.Equal(typeof(Guid), guidresult.GetType());
            Assert.Equal(typeof(TestString), enumresult.GetType());
            Assert.Null(nullResult);
            Assert.Equal(emptyGuid, nullGuidResult);
            Assert.Equal(version, versionResult.ToString());
            Assert.Equal(person, notIconvertableResult);
        }
    }
}