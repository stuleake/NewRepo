using CT.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using static UnitTest.CT.Utils.Extensions.TypeExtensionsTest;

namespace UnitTest.CT.Utils.Extensions
{
#pragma warning disable IDE0051
#pragma warning disable S1144
#pragma warning disable S3459

    /// <summary>
    /// Class for Reflection Extensions unitTest
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ReflectionExtensionsTest
    {
        /// <summary>
        /// Test interface
        /// </summary>
        private interface ITest1
        {
            /// <summary>
            /// Gets or Sets string property
            /// </summary>
            string StringProperty { get; set; }

            /// <summary>
            /// Gets or Sets integer property
            /// </summary>
            int IntegerProperty { get; set; }
        }

        /// <summary>
        /// Interface for Test2
        /// </summary>
        private interface ITest2 : ITest1
        {
            /// <summary>
            /// Gets or Sets Test class object property
            /// </summary>
            TestClass TestClassObjectProperty { get; set; }

            /// <summary>
            /// returns string
            /// </summary>
            /// <returns>string</returns>
            string ReturnString();

            /// <summary>
            /// returns None
            /// </summary>
            void ReturnNone();

            /// <summary>
            /// returns object
            /// </summary>
            /// <returns>object</returns>
            object ReturnObject();
        }

        private class TestClass1
        {
            [Key]
            public string StringProperty { get; set; }

            public string ReadOnlyProperty { get; }

            public TestClass2 ObjectProperty { get; set; }

            private string PrivateProperty { get; set; }

            protected string ProtectedProperty { get; set; }

            public static string GetGenericClassName<T>(T testobject)
                where T : class
            {
                if (testobject == null)
                {
                    throw new ArgumentNullException(nameof(testobject));
                }

                return typeof(T).Name;
            }

            public static string GetGenericClassNames<T1, T2>(T1 object1, T2 object2)
                where T1 : class
                where T2 : class
            {
                if (object1 == null)
                {
                    throw new ArgumentNullException(nameof(object1));
                }

                if (object2 == null)
                {
                    throw new ArgumentNullException(nameof(object2));
                }

                return typeof(T1).Name + typeof(T2).Name;
            }
        }

        private class SourceClass
        {
            [CustomAttribute]
            public string StringProperty { get; set; }

            public int? IntegerProperty { get; set; }

            public TestClass ObjectProperty { get; set; }

            public string ReadOnlyProperty { get; set; }

            public Guid GuidProperty { get; set; }
        }

        private class DestinationClass
        {
            public string StringProperty { get; set; }

            public int IntegerProperty { get; set; }

            public TestClass ObjectProperty { get; set; }

            public static string ReadOnlyProperty
            {
                get
                {
                    return "it's readonly";
                }
            }

            public Guid GuidProperty { get; set; }
        }

        private class TestClass2
        {
        }

        [AttributeUsage(AttributeTargets.All)]
        private sealed class CustomAttribute : Attribute
        {
        }

        /// <summary>
        /// Get Module
        /// </summary>
        [Fact]
        public void GetModuleTest()
        {
            // Arrange
            var type = typeof(TestClass);

            // Act
            var result = type.GetModule();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(typeof(TestClass).Module, result);
        }

        /// <summary>
        /// Test for get module null check
        /// </summary>
        [Fact]
        public void GetModuleNullTest()
        {
            // Arrange
            Type type = null;

            // Act
            var result = type.GetModule();

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Unit Test for Get all properties
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="expectedNumberOfProperties">expected number</param>
        [Theory]
        [InlineData(typeof(ITest2), 3)]
        [InlineData(typeof(TestClass1), 5)]
        public void GetAllPropertiesTest(Type type, int expectedNumberOfProperties)
        {
            // Act
            var result = type.GetAllProperties();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedNumberOfProperties, result.Length);
        }

        /// <summary>
        /// Unit Test for Get public properties
        /// </summary>
        /// <param name="type">type</param>
        /// <param name="expectedNumberofProperties">expected number</param>
        [Theory]
        [InlineData(typeof(ITest2), 3)]
        [InlineData(typeof(TestClass1), 3)]
        public void GetPublicPropertiesTest(Type type, int expectedNumberofProperties)
        {
            // Act
            var result = type.GetPublicProperties();

            // Assert
            Assert.Equal(expectedNumberofProperties, result.Length);
        }

        /// <summary>
        /// Get Atrribute Unit test
        /// </summary>
        [Fact]
        public void GetAttributeFromTest()
        {
            // Arrange
            var builtinattributetest = new TestClass1();
            var customattributetest = new SourceClass();

            // Act
            var builtinattributeresult = builtinattributetest.GetAttributeFrom<KeyAttribute>("StringProperty");
            var customattributeresult = customattributetest.GetAttributeFrom<CustomAttribute>("StringProperty");

            // Assert
            Assert.NotNull(builtinattributeresult);
            Assert.NotNull(customattributeresult);
            Assert.Equal(typeof(KeyAttribute), builtinattributeresult.GetType());
            Assert.Equal(typeof(CustomAttribute), customattributeresult.GetType());
        }

        /// <summary>
        /// method to copy properties
        /// </summary>
        [Fact]
        public void CopyPropertiesToTest()
        {
            // Arrange
            var sourceobject = new SourceClass();
            var destinationobject = new DestinationClass();
            sourceobject.StringProperty = "hello";
            sourceobject.ReadOnlyProperty = "checkvalueforreadonly";
            sourceobject.IntegerProperty = null;
            sourceobject.ObjectProperty = new TestClass();
            sourceobject.GuidProperty = Guid.NewGuid();

            // Act
            sourceobject.CopyPropertiesTo(destinationobject, true);

            // Assert
            Assert.Equal(sourceobject.StringProperty, destinationobject.StringProperty);
            Assert.Equal(0, destinationobject.IntegerProperty); // Checking Do Not copy null value
            Assert.Equal(sourceobject.ObjectProperty, destinationobject.ObjectProperty);
            Assert.Equal("it's readonly", DestinationClass.ReadOnlyProperty); // Checking for read only property
            Assert.Equal(sourceobject.GuidProperty, destinationobject.GuidProperty);
        }

        /// <summary>
        /// method to create object unit test
        /// </summary>
        [Fact]
        public void CreateObjectTest()
        {
            // Arrange
            var currentAssemblyClassName = $"{nameof(ReflectionExtensionsTest)}+{nameof(TestClass1)}";

            // Act
            var currentassemblyresult = currentAssemblyClassName.CreateObject<TestClass1>("UnitTest.CT.Utils", "Extensions", true);

            // Assert
            Assert.NotNull(currentassemblyresult);
            Assert.Equal(typeof(TestClass1), currentassemblyresult.GetType());
        }

        /// <summary>
        /// Method to get the tpe
        /// </summary>
        /// <param name="className">class Name</param>
        /// <param name="assemblyName">assembly name</param>
        /// <param name="nameSpace">namespace</param>
        /// <param name="expectedType">expected type</param>
        [Theory]
        [InlineData("TestClass1", "UnitTest.CT.Utils", "Extensions", typeof(TestClass1))]
        [InlineData("TypeExtensionsTest", "UnitTest.CT.Utils", "Extensions", typeof(TypeExtensionsTest))]
        public void GetTypeTest(string className, string assemblyName, string nameSpace, Type expectedType)
        {
            // Act
            var result = className.GetType(assemblyName, nameSpace);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedType, result);
        }

        /// <summary>
        /// method to set the object properties from keyvalur Pair
        /// </summary>
        [Fact]
        public void SetObjectPropertiesFromKeyvaluePairTest()
        {
            // Arrange
            var kvpairlist = new List<KeyValuePair<string, object>> { };
            var testobject = new SourceClass();
            var stringvalue = "teststring";
            var intvalue = 100;
            var objectvalue = new TestClass();
            var guidvalue = Guid.NewGuid();
            kvpairlist.Add(new KeyValuePair<string, object>(nameof(testobject.StringProperty), stringvalue));
            kvpairlist.Add(new KeyValuePair<string, object>(nameof(testobject.IntegerProperty), intvalue));
            kvpairlist.Add(new KeyValuePair<string, object>(nameof(testobject.ObjectProperty), objectvalue));
            kvpairlist.Add(new KeyValuePair<string, object>(nameof(testobject.GuidProperty), guidvalue));

            // Act
            testobject.SetObjectPropertiesFromKeyValuePair(kvpairlist);

            // Assert
            Assert.NotNull(testobject.StringProperty);
            Assert.NotNull(testobject.IntegerProperty);
            Assert.NotNull(testobject.ObjectProperty);
            Assert.Equal(stringvalue, testobject.StringProperty);
            Assert.Equal(intvalue, testobject.IntegerProperty);
            Assert.Equal(objectvalue, testobject.ObjectProperty);
            Assert.Equal(guidvalue, testobject.GuidProperty);
        }

        /// <summary>
        /// Invoke Generic method unit test
        /// </summary>
        [Fact]
        public void InvokeGenericMethodTest()
        {
            // Arrange
            var testobject = new TestClass1();

            // Act
            var result = testobject.InvokeGenericMethod(this.GetType(), nameof(testobject.GetGenericClassName), new object[] { this });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(this.GetType().Name, result);
        }

        /// <summary>
        /// Method to Invoke Generic Method with more than one generic types unit Test
        /// </summary>
        [Fact]
        public void InvokeGenericMethodwithmorethanonegenerictypesTest()
        {
            // Arrange
            var testobject = new TestClass1();
            var expectedresult = typeof(TestClass1).Name + typeof(TestClass2).Name;

            // Act
            var result = testobject.InvokeGenericMethod(
                new Type[] { typeof(TestClass1), typeof(TestClass2) },
                nameof(testobject.GetGenericClassNames),
                new TestClass1(),
                new TestClass2());

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedresult, result);
        }

        /// <summary>
        /// methos to set property
        /// </summary>
        [Fact]
        public void TrySetPropertyTest()
        {
            // Arrange
            var testobject = new TestClass1();
            var stringvalue = "hello";
            var objectvalue = new TestClass2();

            // Act
            testobject.TrySetProperty(nameof(testobject.StringProperty), stringvalue);
            testobject.TrySetProperty(nameof(testobject.ObjectProperty), objectvalue);

            // Assert
            Assert.NotNull(testobject.StringProperty);
            Assert.Equal(stringvalue, testobject.StringProperty);
            Assert.Equal(objectvalue, testobject.ObjectProperty);
        }

        /// <summary>
        /// Test case for null validation in method GetAllProperties
        /// </summary>
        [Fact]
        public void GetAllPropertiesNullTest()
        {
            // Arrange
            Type type = null;

            // Act
            Exception error = null;
            try
            {
                _ = type.GetAllProperties();
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Test case for null validation in method GetPublicProperties
        /// </summary>
        [Fact]
        public void GetPublicPropertiesNullTest()
        {
            // Arrange
            Type type = null;

            // Act
            Exception error = null;
            try
            {
                _ = type.GetPublicProperties();
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Test case for null validation in method GetAttributeFrom
        /// </summary>
        [Fact]
        public void GetAttributeFromNullTest()
        {
            // Arrange
            Type type = null;

            // Act
            Exception error = null;
            try
            {
                _ = type.GetAttributeFrom<KeyAttribute>("StringProperty");
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Test case for null validation in method CopyPropertiesTo
        /// </summary>
        [Fact]
        public void CopyPropertiesToNullTest()
        {
            // Arrange
            SourceClass sourceClass = new SourceClass();
            SourceClass sourceClassNull = null;
            DestinationClass destinationClassNull = null;

            // Act
            Exception sourceError = null;
            Exception destinationError = null;
            try
            {
                sourceClassNull.CopyPropertiesTo(destinationClassNull);
            }
            catch (ArgumentNullException ex)
            {
                destinationError = ex;
            }
            try
            {
                sourceClass.CopyPropertiesTo(destinationClassNull);
            }
            catch (ArgumentNullException ex)
            {
                sourceError = ex;
            }

            // Assert
            Assert.NotNull(sourceError);
            Assert.NotNull(destinationError);
        }

        /// <summary>
        /// Test case for null validation in method SetObjectPropertiesFromKeyvaluePair
        /// </summary>
        [Fact]
        public void SetObjectPropertiesFromKeyvaluePairNullTest()
        {
            // Arrange
            List<KeyValuePair<string, object>> kvpairlist = null;
            SourceClass sourceClass = new SourceClass();
            SourceClass sourceClassNull = null;

            // Act
            Exception error1 = null;
            Exception error2 = null;
            try
            {
                sourceClassNull.SetObjectPropertiesFromKeyValuePair(kvpairlist);
            }
            catch (ArgumentNullException ex)
            {
                error1 = ex;
            }
            try
            {
                sourceClass.SetObjectPropertiesFromKeyValuePair(kvpairlist);
            }
            catch (ArgumentNullException ex)
            {
                error2 = ex;
            }

            // Assert
            Assert.NotNull(error1);
            Assert.NotNull(error2);
        }

        /// <summary>
        /// Test case for null validation in method InvokeGenericMethod
        /// </summary>
        [Fact]
        public void InvokeGenericMethodNullTest()
        {
            // Arrange
            TestClass1 testobject = null;

            // Act
            Exception error = null;
            try
            {
                testobject.InvokeGenericMethod(this.GetType(), nameof(testobject.GetGenericClassName), new object[] { this });
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        /// <summary>
        /// Test case for null validation in method TrySetProperty
        /// </summary>
        [Fact]
        public void TrySetPropertyNullTest()
        {
            // Arrange
            TestClass1 testobject = null;
            var stringvalue = "hello";
            Exception error = null;

            // Act
            try
            {
                testobject.TrySetProperty(nameof(testobject.StringProperty), stringvalue);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }
    }

#pragma warning restore IDE0051
#pragma warning restore S1144
#pragma warning restore S3459
}