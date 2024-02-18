using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text.Json;

namespace huzcodes.Extensions.Exceptions
{
    public static class Handlers
    {
        /// <summary>
        /// Registring exception handler of the plugin, 
        /// for handling any exception and thrown errors and custom responses, including fluent validation
        /// </summary>
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

                // handling for fluent validation
                if (exception is FluentValidation.ValidationException validationException)
                {
                    var errors = validationException.Errors.Select(error => new
                    {
                        Property = error.PropertyName,
                        Message = error.ErrorMessage
                    });

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorContent = JsonSerializer.Serialize(errors);
                }


                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(errorContent, System.Text.Encoding.UTF8);
            }));
        }

        /// <summary>
        /// Registring fluent validation in extension plugin, 
        /// in case you want to use fluent validation in your solution,
        /// and get handled in the exception handler of the plugin.
        /// </summary>
        /// <param name="contextType"></param>
        public static void AddFluentValidation(this IServiceCollection services, Type contextType)
        {
            services.AddValidatorsFromAssembly(contextType.Assembly);
        }
    }
}
