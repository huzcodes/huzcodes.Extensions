using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace huzcodes.Extensions.Exceptions
{
    public static class Handlers
    {
        public static void AddExceptionHandlerExtension(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(o =>
            o.Run(async context =>
            {
                var errorContent = string.Empty;
                var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = errorFeature!.Error;

                if (!(exception is ResultException) && !(exception is CustomResultException<dynamic>))
                    throw exception;

                if (exception is ResultException resultException)
                {
                    errorContent = JsonSerializer.Serialize(resultException);
                    context.Response.StatusCode = resultException.ResultExceptionStatusCode;
                }

                if (exception is CustomResultException<dynamic> customResultException)
                {
                    errorContent = JsonSerializer.Serialize(customResultException);
                    context.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                }

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorContent, System.Text.Encoding.UTF8);
            }));
        }
    }
}
