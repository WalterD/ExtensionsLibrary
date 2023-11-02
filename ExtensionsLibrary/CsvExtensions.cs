using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Newtonsoft.Json;

namespace ExtensionsLibrary
{
    public static class CsvExtensions
    {
        /// <summary>
        /// Converts collection to a file stream
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="collection">collection</param>
        /// <returns>Stream</returns>
        public static Stream ToFileStream<T>(this ICollection<T> collection, bool firstRowContainsColumnHeaders = true)
        {
            string csvString = collection.ToCsvString(firstRowContainsColumnHeaders);
            csvString ??= string.Empty;
            return new MemoryStream(Encoding.UTF8.GetBytes(csvString));
        }

        /// <summary>
        /// Converts collection to a byte array
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="collection">collection</param>
        /// <returns>Byte array</returns>
        public static byte[] ToCsvByteArray<T>(this ICollection<T> collection, bool firstRowContainsColumnHeaders = true)
        {
            string csvString = collection.ToCsvString(firstRowContainsColumnHeaders);
            csvString ??= string.Empty;
            return Encoding.UTF8.GetBytes(csvString);
        }

        /// <summary>
        /// Save collection to a file as a CSV
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="collection">Collection of objects</param>
        /// <param name="filePath">Destination file path. The file should have extension of .csv</param>
        /// <returns>csvString</returns>
        public static async Task<string> ToCsvFileAsync<T>(this ICollection<T> collection, string filePath, bool firstRowContainsColumnHeaders = true)
        {
            string csvString = collection.ToCsvString(firstRowContainsColumnHeaders);
            csvString ??= string.Empty;
            await File.WriteAllTextAsync(filePath, csvString).ConfigureAwait(false);
            return csvString;
        }

        /// <summary>
        /// Save Json collection to a file as a CSV
        /// </summary>
        /// <param name="jsonContent">Collection of objects as Json string</param>
        /// <param name="filePath">Destination file path. The file should have extension of .csv</param>
        /// <returns>csvString</returns>
        public static async Task<string> ToCsvFileAsync(this string jsonContent, string filePath, bool firstRowContainsColumnHeaders = true)
        {
            string csvString = jsonContent.ToCsvString(firstRowContainsColumnHeaders);
            csvString ??= string.Empty;
            await File.WriteAllTextAsync(filePath, csvString).ConfigureAwait(false);
            return csvString;
        }

        /// <summary>
        /// Convert collection to a CSV format
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="collection">Collection of objects</param>
        /// <returns>string</returns>
        public static string ToCsvString<T>(this ICollection<T> collection, bool firstRowContainsColumnHeaders = true)
        {
            var properties = typeof(T).GetProperties().ToList();
            using var stringWriter = new StringWriter();
            using var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);
            if (firstRowContainsColumnHeaders)
            {
                // loop thru each property in the class
                foreach (var property in properties)
                {
                    // check if property is decorated with display attribute. If so, get the header name from there
                    string columnName;
                    var attr = property.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (!attr.IsNullOrEmpty())
                    {
                        columnName = ((DisplayAttribute)attr.First()).Name;
                    }
                    else
                    {
                        columnName = property.Name;
                    }

                    csvWriter.WriteField(columnName);
                }

                csvWriter.NextRecord();
            }

            // if there are no records, exit
            if (collection == null || collection.Count == 0)
            {
                return stringWriter.ToString();
            }

            // loop thru all records
            foreach (var item in collection)
            {
                // loop thru all fields
                foreach (var property in properties)
                {
                    // get field value
                    var propertyValue = property.GetValue(item);

                    // write field value to the CSV sheet
                    csvWriter.WriteField(propertyValue);
                }

                csvWriter.NextRecord();
            }

            return stringWriter.ToString();
        }

        /// <summary>
        /// Convert Json collection to a CSV format
        /// </summary>
        /// <param name="jsonContent">Collection of objects as Json string</param>
        /// <returns>string</returns>
        public static string ToCsvString(this string jsonContent, bool firstRowContainsColumnHeaders = true)
        {
            using var stringWriter = new StringWriter();
            using var csvWriter = new CsvWriter(stringWriter, CultureInfo.InvariantCulture);

            // convert Json string to DataTable
            using (var table = JsonConvert.DeserializeObject<DataTable>(jsonContent))
            {
                if (firstRowContainsColumnHeaders)
                {
                    // create header names from column names
                    foreach (DataColumn column in table.Columns)
                    {
                        csvWriter.WriteField(column.ColumnName);
                    }

                    csvWriter.NextRecord();
                }

                // loop thru each record in the table
                foreach (DataRow row in table.Rows)
                {
                    // loop thru each column in the record
                    for (var i = 0; i < table.Columns.Count; i++)
                    {
                        // write field value to the CSV sheet
                        csvWriter.WriteField(row[i]);
                    }

                    csvWriter.NextRecord();
                }
            }

            return stringWriter.ToString();
        }
    }
}
