using huzcodes.Extensions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace huzcodes.Extensions.Identity
{
    public class IdentityKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "X-Api-Key";
        public IdentityKeyMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
                throw new ResultException("Unauthorized! you must provide the API Key.",
                                          (int)HttpStatusCode.Unauthorized);

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEYNAME);

            if (!string.IsNullOrEmpty(apiKey) && !apiKey.Equals(extractedApiKey))
                throw new ResultException("Unauthorized! you must provide a vaild API Key.",
                                          (int)HttpStatusCode.Unauthorized);

            await _next(context);
        }

        public async Task InvokeAsyncUp(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                await _next(context);
            else
            {
                if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
                    throw new ResultException("Unauthorized! you must provide the API Key.",
                                              (int)HttpStatusCode.Unauthorized);

                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var apiKey = appSettings.GetValue<string>(APIKEYNAME);

                if (!string.IsNullOrEmpty(apiKey) && !apiKey.Equals(extractedApiKey))
                    throw new ResultException("Unauthorized! you must provide a vaild API Key.",
                                              (int)HttpStatusCode.Unauthorized);

                await _next(context);
            }
        }

        public async Task InvokeAsyncUpN(HttpContext context)
        {
            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEYNAME);

            if (!string.IsNullOrEmpty(apiKey) && apiKey.Equals(context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey)))
                await _next(context);

            if (context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
                await _next(context);

            throw new ResultException("Unauthorized! you must provide a vaild API Key.",
                                          (int)HttpStatusCode.Unauthorized);
        }
    }
}
