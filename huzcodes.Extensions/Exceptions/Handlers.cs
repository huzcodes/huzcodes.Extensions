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

                // handling for default exception
                if (!(exception is ResultException) && !(exception is CustomResultException))
                    errorContent = JsonSerializer.Serialize(new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = exception.Message,
                        InnerException = exception.InnerException,
                        Data = exception.Data,
                        Type = exception.GetType().Name
                    });

                // handling for ResultException
                if (exception is ResultException)
                {
                    context.Response.StatusCode = ((ResultException)exception).ResultExceptionStatusCode;
                    errorContent = JsonSerializer.Serialize(new
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = ((ResultException)exception).ResultExceptionMessage,
                        Type = exception.GetType().Name
                    });
                }

                // handling for CustomResultException
                if (exception is CustomResultException)
                {
                    errorContent = JsonSerializer.Serialize(((dynamic)exception).CustomExceptionContract);
                    context.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                }


                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorContent, System.Text.Encoding.UTF8);
            }));
        }
    }
}
