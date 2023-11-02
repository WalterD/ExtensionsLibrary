using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace ExtensionsLibrary
{
    public static class JsonExtensions
    {
        /// <summary>
        /// Writes to file as json.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns>Json string written to the file</returns>
        public static string WriteToFileAsJson(this object obj, string filePath, Formatting formatting)
        {
            string jsonString = obj.ToJsonString(formatting);
            File.WriteAllText(filePath, jsonString);
            return jsonString;
        }

        /// <summary>
        /// WriteToFileAsJson
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="filePath">filePath</param>
        /// <param name="formatting">formatting</param>
        /// <param name="useCamelCase">useCamelCase</param>
        /// <returns>Json string written to the file</returns>
        public static string WriteToFileAsJson(this object obj, string filePath, Formatting formatting, bool useCamelCase)
        {
            string jsonString = obj.ToJsonString(formatting, useCamelCase);
            File.WriteAllText(filePath, jsonString);
            return jsonString;
        }

        /// <summary>
        /// Reads the json from file.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns>Object of type T</returns>
        public static T ReadJsonFromFile<T>(this string filePath) where T : class
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// ReadJsonFromFile
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filePath">filePath</param>
        /// <param name="useCamelCase">useCamelCase</param>
        /// <returns>Object of type T</returns>
        public static T ReadJsonFromFile<T>(this string filePath, bool useCamelCase) where T : class
        {
            string jsonString = File.ReadAllText(filePath);

            if (jsonString.TryDeserializeObject(useCamelCase, out T obj))
            {
                return obj;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Reads the json from file asynchronous.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <returns>Task</returns>
        public static async Task<T> ReadJsonFromFileAsync<T>(this string filePath) where T : class
        {
            string jsonString = await File.ReadAllTextAsync(filePath);
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// To the json string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns>string</returns>
        public static string ToJsonString(this object obj, Formatting formatting)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }

        /// <summary>
        /// Convert object to Json string
        /// </summary>
        /// <param name="obj">object</param>
        /// <param name="formatting">formatting</param>
        /// <param name="useCamelCase">useCamelCase</param>
        /// <returns>Json string</returns>
        public static string ToJsonString(this object obj, Formatting formatting, bool useCamelCase)
        {
            JsonSerializerSettings serializerSettings = null;
            if (useCamelCase)
            {
                serializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            }

            string jsonString = JsonConvert.SerializeObject(obj, formatting, serializerSettings);
            return jsonString;
        }

        /// <summary>
        /// To the json string.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns>string</returns>
        public static string ToJsonStringExcludeNullFields(this object obj, Formatting formatting)
        {
            return JsonConvert.SerializeObject(
                obj,
                new JsonSerializerSettings
                {
                    Formatting = formatting,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        /// <summary>
        /// Json the serialize.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns>string</returns>
        public static string JsonSerialize(this object obj, Formatting formatting)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                DateFormatString = "dd/MM/yyy hh:mm:ss"
            };

            return JsonConvert.SerializeObject(obj, formatting, jsonSettings);
        }

        /// <summary>
        /// This function will append to a Json array. It assumes that the last character in the file is a ']'
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="formatting">The formatting.</param>
        /// <param name="fileLock">The file lock.</param>
        public static void WriteJsonToFileAsArrayElement(this object obj, string filePath, Formatting formatting, object fileLock)
        {
            string jsonString = obj.ToJsonString(formatting);
            lock (fileLock)
            {
                if (!File.Exists(filePath))
                {
                    File.WriteAllText(filePath, $"[{Environment.NewLine}{jsonString}{Environment.NewLine}]");
                }
                else
                {
                    FileStream fileStream = null;
                    try
                    {
                        fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                        long newPosition = fileStream.Length;
                        while (newPosition > 0)
                        {
                            fileStream.Position = --newPosition;
                            char d = (char)(byte)fileStream.ReadByte();
                            if (d == 0 || char.IsWhiteSpace(d))
                            {
                                continue;
                            }

                            if (d == ']')
                            {
                                // reset position
                                fileStream.Position = newPosition;

                                // write the new entry
                                byte[] bytesInStream = Encoding.ASCII.GetBytes($",{Environment.NewLine}{jsonString}{Environment.NewLine}]");
                                fileStream.Write(bytesInStream, 0, bytesInStream.Length);
                            }

                            break;
                        }
                    }
                    finally
                    {
                        if (fileStream != null)
                        {
                            fileStream.Close();
                            fileStream.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clones the object.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>T</returns>
        public static T Clone<T>(this T source)
        {
            if (source == null)
            {
                return default;
            }

            string objSerialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(objSerialized);
        }

        /// <summary>
        /// Formats the json.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="formatting">The formatting.</param>
        /// <returns>Formatted json</returns>
        public static string FormatJson(this string json, Formatting formatting)
        {
            return JToken.Parse(json).ToString(formatting);
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="jsonString">The json string.</param>
        /// <returns>Deserialized object</returns>
        public static T DeserializeObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        /// <summary>
        /// Tries the deserialize object.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="jsonString">The json string.</param>
        /// <param name="obj">The object.</param>
        /// <returns>Operation status</returns>
        public static bool TryDeserializeObject<T>(this string jsonString, out T obj)
        {
            try
            {
                obj = JsonConvert.DeserializeObject<T>(jsonString);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// TryDeserializeObject
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="jsonString">jsonString</param>
        /// <param name="useCamelCase">useCamelCase</param>
        /// <param name="obj">object</param>
        /// <returns>true/false</returns>
        public static bool TryDeserializeObject<T>(this string jsonString, bool useCamelCase, out T obj)
        {
            try
            {
                JsonSerializerSettings serializerSettings = null;
                if (useCamelCase)
                {
                    serializerSettings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };
                }

                obj = JsonConvert.DeserializeObject<T>(jsonString, serializerSettings);
                return true;
            }
            catch (System.Exception)
            {
                obj = default;
                return false;
            }
        }

        /// <summary>
        /// Gets the name of the json property.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>json property name</returns>
        public static string GetJsonPropertyName<T>(this T obj, string propertyName)
        {
            return obj.GetType()
                        .GetProperty(propertyName)
                        .GetCustomAttribute<JsonPropertyAttribute>()
                        .PropertyName;
        }

        /// <summary>
        /// Gets the json property attributes.
        /// </summary>
        /// <param name="t">The t.</param>
        /// <returns>Dictionary</returns>
        public static Dictionary<string, string> GetJsonPropertyAttributes(this Type t)
        {
            var dict = new Dictionary<string, string>();
            PropertyInfo[] props = t.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                var attr = prop.GetCustomAttribute<JsonPropertyAttribute>();
                dict.Add(prop.Name, attr.PropertyName);
            }

            return dict;
        }

        /// <summary>
        /// Json2s the data table.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>DataTable</returns>
        public static DataTable Json2DataTable(this string json)
        {
            var dt = JsonConvert.DeserializeObject<DataTable>(json);
            return dt;
        }

        /// <summary>
        /// Copies the object values from json. Property names are not case-sensitive.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="obj">Destination object</param>
        /// <param name="json">The json.</param>
        /// <returns>Object of type T</returns>
        public static T CopyObjectValuesFromJson<T>(this T obj, string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return obj;
            }

            Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            var properties = typeof(T).GetProperties().Where(p => p.CanWrite);
            if (dictionary.Any() && properties.Any())
            {
                foreach (var propertyName in dictionary.Keys)
                {
                    var property = properties.FirstOrDefault(p => p.Name.ToUpper() == propertyName.ToUpper());
                    if (property != null)
                    {
                        property.SetValue(obj, dictionary[propertyName]);
                    }
                }
            }

            return obj;
        }
    }
}
