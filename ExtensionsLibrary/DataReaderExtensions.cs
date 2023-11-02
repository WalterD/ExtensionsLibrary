using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace ExtensionsLibrary
{
    public static class DataReaderExtensions
    {
        /// <summary>
        /// Data the reader map to list.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="dr">The dr.</param>
        /// <returns>List of T</returns>
        public static List<T> DataReaderMapToList<T>(this IDataReader dr)
        {
            var list = new List<T>();
            List<string> readerFields = null;
            while (dr.Read())
            {
                // create list of fields that exist in the reader
                if (readerFields == null)
                {
                    readerFields = new List<string>();
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        readerFields.Add(dr.GetName(i).ToLower());
                    }
                }

                T obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (readerFields.Contains(prop.Name.ToLower()))
                    {
                        try
                        {
                            object value = dr[prop.Name];
                            if (Equals(value, DBNull.Value))
                            {
                                prop.SetValue(obj, null, null);
                            }
                            else
                            {
                                prop.SetValue(obj, value, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            string conversionInfo = $"PropertyName={prop.Name}, DestinationPropertyType={prop.PropertyType}, SourcePropertyType{(dr[prop.Name]).GetType()}:";
                            FieldInfo message = ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
                            object value = message.GetValue(ex);
                            message.SetValue(ex, $"{value}, ConversionInfo: {conversionInfo}");
                            throw;
                        }
                    }
                }

                list.Add(obj);
            }

            return list;
        }
    }
}
