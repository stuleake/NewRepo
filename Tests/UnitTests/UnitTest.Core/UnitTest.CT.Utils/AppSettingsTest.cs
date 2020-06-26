using CT.Utils;
using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace UnitTest.CT.Utils
{
    /// <summary>
    /// Application Settings Test
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AppSettingsTest
    {
        /// <summary>
        /// Get string type setting
        /// </summary>
        [Fact]
        public void GetStringTest()
        {
            // Assert
            Assert.Equal("DevelopmentTestString", ApplicationSettings.GetAppSetting<string>("TestKeyString", "globals", "Development"));
        }

        /// <summary>
        /// Get string type setting failure
        /// </summary>
        [Fact]
        public void GetStringFailureTest()
        {
            // Assert
            Assert.NotEqual("TestString", ApplicationSettings.GetAppSetting<string>("TestKeyString", "globals", "Development"));
        }

        /// <summary>
        /// Get integer type setting
        /// </summary>
        [Fact]
        public void GetIntegerTest()
        {
            // Assert
            Assert.Equal(100, ApplicationSettings.GetAppSetting<int>("TestKeyInteger", "globals"));
        }

        /// <summary>
        /// Get float type setting
        /// </summary>
        [Fact]
        public void GetFloatTest()
        {
            // Assert
            Assert.Equal(1.0F, ApplicationSettings.GetAppSetting<float>("TestKeyFloat", "globals"));
        }

        /// <summary>
        /// Get double type setting
        /// </summary>
        [Fact]
        public void GetDoubleTest()
        {
            // Assert
            Assert.Equal(1.0D, ApplicationSettings.GetAppSetting<double>("TestKeyFloat", "globals"));
        }

        /// <summary>
        /// Get boolean type setting
        /// </summary>
        [Fact]
        public void GetBooleanTest()
        {
            // Assert
            Assert.True(ApplicationSettings.GetAppSetting<bool>("TestKeyBoolean", "globals"));
        }

        /// <summary>
        /// Get invalid section test
        /// </summary>
        [Fact]
        public void GetInvalidSectionTest()
        {
            // Arrange
            Exception error = null;

            // Act
            try
            {
                _ = ApplicationSettings.GetAppSetting<bool>("TestKeyBoolean", "InvalidSection");
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
            Assert.True(string.Compare(error.Message, "Couldn't find the section or key: 'InvalidSection' > 'TestKeyBoolean' in appsettings.json. Please check the configuration file.", true) == 0);
        }

        /// <summary>
        /// Get exception null section test
        /// </summary>
        [Fact]
        public void GetExceptionWithNullSectionNameTest()
        {
            // Assert
            var exception = Assert.Throws<SettingsPropertyNotFoundException>(() => ApplicationSettings.GetAppSetting<string>("TestKeyString", string.Empty, "Development"));
            Assert.Equal("The sectionName in appsettings.Development.json cannot be empty.. Please check the configuration file.", exception.Message);
        }

        /// <summary>
        /// Get exception invalid section test
        /// </summary>
        [Fact]
        public void GetExceptionWithInvalidSectionNameTest()
        {
            // Assert
            var exception = Assert.Throws<SettingsPropertyNotFoundException>(() => ApplicationSettings.GetAppSetting<string>("TestKeyString", "InvalidSection"));
            Assert.Equal("Couldn't find the section or key: 'InvalidSection' > 'TestKeyString' in appsettings.json. Please check the configuration file.", exception.Message);
        }

        /// <summary>
        /// Get exception invalid key test
        /// </summary>
        [Fact]
        public void GetExceptionWithInvalidKeyNameTest()
        {
            // Assert
            var exception = Assert.Throws<SettingsPropertyNotFoundException>(() => ApplicationSettings.GetAppSetting<string>("InvalidTestKeyString", "globals"));
            Assert.Equal("Couldn't find the section or key: 'globals' > 'InvalidTestKeyString' in appsettings.json. Please check the configuration file.", exception.Message);
        }

        /// <summary>
        /// Get exception invalid filename test
        /// </summary>
        [Fact]
        public void InvalidFileNameTest()
        {
            // Arrange
            Exception error = null;

            // Act
            try
            {
                _ = ApplicationSettings.GetAppSetting<bool>("TestKeyBoolean", "globals", "Test");
            }
            catch (SettingsPropertyNotFoundException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
            Assert.True(string.Compare(error.Message, "Couldn't find the file appsettings.Test.json. Please check the configuration file.", true) == 0);
        }
    }
}