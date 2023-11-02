using Microsoft.AspNetCore.Http;

namespace ExtensionsLibrary
{
    public static class HttpRequestExtensions
    {
        public static string GetBaseUrl(this HttpContext httpContext)
        {
            var request = httpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $"{request.Scheme}://{host}{pathBase}";
        }
    }
}