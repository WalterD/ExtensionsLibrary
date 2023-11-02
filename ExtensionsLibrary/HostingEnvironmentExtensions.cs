using Microsoft.Extensions.Hosting;

namespace ExtensionsLibrary
{
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// IsLocalhost
        /// </summary>
        /// <param name="hostingEnvironment">hostingEnvironment</param>
        /// <returns>bool</returns>
        public static bool IsLocalhost(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.ToLower() == "localhost";
        }

        /// <summary>
        /// IsQA
        /// </summary>
        /// <param name="hostingEnvironment">hostingEnvironment</param>
        /// <returns>bool</returns>
        public static bool IsQA(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.ToLower() == "qa";
        }
    }
}
