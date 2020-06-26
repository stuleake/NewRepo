using CT.Utils.Extensions;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using DataTableExtensions = CT.Utils.Extensions.DataTableExtensions;

namespace UnitTest.CT.Utils.Extensions
{
    [ExcludeFromCodeCoverage]
    public class DataTableExtensionsTest
    {
        internal class NameValuePair
        {
            public string Name { get; set; }

            public string Value { get; set; }
        }

        [Fact]
        public void MapToListTest()
        {
            // Setting up the table
            var tempTable = new DataTable();
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Name" });
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Value" });
            tempTable.Rows.Add("Name1", "Value1");
            tempTable.Rows.Add("Name2", "Value2");

            // Setting Up list
            var tempList = tempTable.MapToList<NameValuePair>();
            var tempCount = tempList.Count();

            // Testing counts
            Assert.Equal(tempTable.Rows.Count, tempCount);

            // Testing the values
            Assert.Equal(Convert.ToString(tempTable.Rows[0][0]), tempList.ElementAt(0).Name);
            Assert.Equal(Convert.ToString(tempTable.Rows[0][1]), tempList.ElementAt(0).Value);
            Assert.Equal(Convert.ToString(tempTable.Rows[1][0]), tempList.ElementAt(1).Name);
            Assert.Equal(Convert.ToString(tempTable.Rows[1][1]), tempList.ElementAt(1).Value);
        }

        [Fact]
        public void MapToListUnequalColumnsTest()
        {
            // Setting up the table
            var tempTable = new DataTable();
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Name" });
            tempTable.Rows.Add("Name1");
            tempTable.Rows.Add("Name2");

            // Setting Up list
            var tempList = tempTable.MapToList<NameValuePair>();
            var tempCount = tempList.Count();

            // Testing counts
            Assert.Equal(tempTable.Rows.Count, tempCount);

            // Testing the values
            Assert.Equal(Convert.ToString(tempTable.Rows[0][0]), tempList.ElementAt(0).Name);
            Assert.Equal(Convert.ToString(tempTable.Rows[1][0]), tempList.ElementAt(1).Name);
        }

        [Fact]
        public void MapToListFailure()
        {
            // Act
            var tempList = new DataTable().MapToList<NameValuePair>();

            // Assert
            Assert.Empty(tempList);
        }

        /// <summary>
        /// Unit test case for null validation of MapToList method
        /// </summary>
        [Fact]
        public void MapToListNullTest()
        {
            // Arrange
            Exception error = null;

            // Act
            try
            {
                _ = DataTableExtensions.MapToList<NameValuePair>(null);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }

        [Fact]
        public void ToCsvTest()
        {
            // Arrange
            var tempTable = new DataTable();
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Name" });
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Value" });
            tempTable.Rows.Add("Name1", "Value1");
            tempTable.Rows.Add("Name2", "Value2");

            // Act
            var csvString = tempTable.ToCsv(",");

            // Assert
            Assert.Equal("Name,Value\r\nName1,Value1\r\nName2,Value2\r\n", csvString);
        }

        [Fact]
        public void ToCsvTestWithoutHeader()
        {
            // Arrange
            var tempTable = new DataTable();
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Name" });
            tempTable.Columns.Add(new DataColumn { DataType = typeof(string), ColumnName = "Value" });
            tempTable.Rows.Add("Name1", "Value1");
            tempTable.Rows.Add("Name2", "Value2");

            // Act
            var csvString = tempTable.ToCsv(",", false);

            // Assert
            Assert.Equal("Name1,Value1\r\nName2,Value2\r\n", csvString);
        }

        /// <summary>
        /// Unit test case for null validation of ToCsvNullTest method
        /// </summary>
        [Fact]
        public void ToCsvNullTest()
        {
            // Arrange
            Exception error = null;

            // Act
            try
            {
                _ = DataTableExtensions.ToCsv(null, ",", true);
            }
            catch (ArgumentNullException ex)
            {
                error = ex;
            }

            // Assert
            Assert.NotNull(error);
        }
    }
}