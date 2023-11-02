using Microsoft.AspNetCore.Http;

namespace ExtensionsLibrary
{
    public static class PageModelExtensions
    {
        public static string GetIpAddress(this HttpContext httpContext)
        {
            return httpContext?.Connection?.RemoteIpAddress?.ToString();
        }
    }
}
