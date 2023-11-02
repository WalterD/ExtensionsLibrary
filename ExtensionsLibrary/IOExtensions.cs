using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExtensionsLibrary
{
    public static class IOExtensions
    {
        /// <summary>
        /// Combines the paths.
        /// </summary>
        /// <param name="path1">The path1.</param>
        /// <param name="path2">The path2.</param>
        /// <returns>Combined Paths</returns>
        public static string CombinePaths(string path1, string path2)
        {
            var pathSeparator = '\\';
            path1 = (path1 ?? string.Empty).Trim().TrimEnd(pathSeparator);
            path2 = (path2 ?? string.Empty).Trim().TrimStart(pathSeparator);
            return $"{path1}{pathSeparator}{path2}";
        }

        /// <summary>
        /// GetApplicationDirectory
        /// </summary>
        /// <returns>Application Directory</returns>
        public static string GetApplicationDirectory()
        {
            var applicationPath = System.Reflection.Assembly.GetEntryAssembly().Location;
            return System.IO.Path.GetDirectoryName(applicationPath);
        }

        /// <summary>
        /// WriteToLogFileAsync. This method does not throw an error. The error, or an empty string,  is returned.
        /// </summary>
        /// <param name="filePath">filePath</param>
        /// <param name="text">text</param>
        /// <param name="semaphoreSlim">semaphoreSlim</param>
        /// <param name="numberOfReTries">numberOfReTries</param>
        /// <param name="delayBetweenReTriesInMiliseconds">delayBetweenReTriesInMiliseconds</param>
        /// <returns>Error thrown or empty string</returns>
        public static async Task<string> WriteToLogFileAsync(string filePath, string text, SemaphoreSlim semaphoreSlim = null, int numberOfReTries = 10, int delayBetweenReTriesInMiliseconds = 100)
        {
            // Create directory if it does not exist.
            try
            {
                if (semaphoreSlim != null)
                {
                    await semaphoreSlim.WaitAsync().ConfigureAwait(false);
                }

                var logDirectory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                if (semaphoreSlim != null)
                {
                    semaphoreSlim.Release();
                }
            }

            numberOfReTries = numberOfReTries > 0 ? numberOfReTries : 1;
            text ??= string.Empty;
            if (!text.IsNullOrWhiteSpace())
            {
                text = $"{ DateTime.Now:s} :: {text.Trim()}{Environment.NewLine}";
            }
            
            byte[] byteArray = new UTF8Encoding().GetBytes(text);
            for (int i = 0; i < numberOfReTries; i++)
            {
                try
                {
                    if (semaphoreSlim != null)
                    {
                        await semaphoreSlim.WaitAsync().ConfigureAwait(false);
                    }

                    // Write to file.
                    using var fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
                    await fs.WriteAsync(byteArray, 0, byteArray.Length).ConfigureAwait(false);
                    fs.Close();
                    break;
                }
                catch (Exception ex)
                {
                    // Throw an exception on the last error.
                    if (i >= numberOfReTries - 1)
                    {
                        return ex.ToString();
                    }
                }
                finally
                {
                    if (semaphoreSlim != null)
                    {
                        semaphoreSlim.Release();
                    }
                }

                if (delayBetweenReTriesInMiliseconds > 0)
                {
                    await Task.Delay(delayBetweenReTriesInMiliseconds).ConfigureAwait(false);
                }
            }

            return string.Empty;
        }
    }
}
