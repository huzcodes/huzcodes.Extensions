using Microsoft.AspNetCore.Http;

namespace huzcodes.Extensions.Identity
{
    public static class HttpExtension
    {
        public static string GetAuthorizationHeader(this IHttpContextAccessor contextAccessor)
        {
            contextAccessor!.HttpContext!.Request.Headers.TryGetValue("Authorization", out var authorizationHeader);
            if (authorizationHeader.ToString().Contains("Bearer"))
                authorizationHeader = authorizationHeader.ToString()
                                                         .Replace("Bearer", "")
                                                         .TrimStart();

            return authorizationHeader!;
        }
    }
}
