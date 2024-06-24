using huzcodes.Extensions.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace huzcodes.Extensions.Identity
{
    public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "X-Api-Key";
        public IdentityMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEYNAME);
            var endpoint = context.GetEndpoint();
            context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey);

            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }

            if (!string.IsNullOrEmpty(apiKey) && apiKey.Equals(extractedApiKey))
            {
                await _next(context);
                return;
            }

            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                await _next(context);
                return;
            }

            throw new ResultException("Unauthorized! you must provide a vaild API Key or a vaild Jwt Token.",
                                      (int)HttpStatusCode.Unauthorized);
        }
    }

}
