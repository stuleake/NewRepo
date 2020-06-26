using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CT.Utils.Extensions
{
    /// <summary>
    /// Class to handle datatable extensions
    /// </summary>
    public static class DataTableExtensions
    {
        /// <summary>
        /// Map the list to a datatable
        /// </summary>
        /// <typeparam name="T">The target list type</typeparam>
        /// <param name="dataTable">The source datatable with which conversion is needed</param>
        /// <returns>Returns a list of objects mapped to a datatable</returns>
        public static IEnumerable<T> MapToList<T>(this DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }
            var list = new List<T>();
            var fields = typeof(T).GetProperties();
            var columnsAvaliable = dataTable.Columns;
            foreach (DataRow dr in dataTable.Rows)
            {
                var instance = Activator.CreateInstance<T>();
                foreach (var field in fields)
                {
                    if (!columnsAvaliable.Contains(field.Name))
                    {
                        continue;
                    }

                    var propertyType = Nullable.GetUnderlyingType(field.PropertyType) ?? field.PropertyType;
                    var value = dr[field.Name];
                    var val = value == DBNull.Value ? null : TypeExtensions.ChangeType(value, propertyType);
                    var safeValue = (value == null) ? null : val;
                    field.SetValue(instance, safeValue, null);
                }

                list.Add(instance);
            }

            return list;
        }

        /// <summary>
        /// Convert the datatable into comma separated value string
        /// </summary>
        /// <param name="datatable">The source datatable to be converted to CSV</param>
        /// <param name="seperator">The separator string for the CSV (generally a single character)</param>
        /// <param name="includeHeader">Include header in CSV</param>
        /// <returns>Returns a comma separated value string converted from the datatable</returns>
        public static string ToCsv(this DataTable datatable, string seperator, bool includeHeader = true)
        {
            if (datatable == null)
            {
                throw new ArgumentNullException(nameof(datatable));
            }
            var sb = new StringBuilder();
            if (includeHeader)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(datatable.Columns[i]);
                    if (i < datatable.Columns.Count - 1)
                    {
                        sb.Append(seperator);
                    }
                }

                sb.AppendLine();
            }

            foreach (DataRow dr in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    var value = dr[i];
                    var val = dr[i] == DBNull.Value ? string.Empty : Convert.ToString(dr[i]);
                    var safeValue = (value == null) ? string.Empty : val;
                    sb.Append(safeValue);

                    if (i < datatable.Columns.Count - 1)
                    {
                        sb.Append(seperator);
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}