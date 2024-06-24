using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace huzcodes.Extensions.Identity
{
    public static class IdentityRegistrations
    {
        /// <summary>
        /// Adding registration for JWT Authorization with scheme Bearer.
        /// Adding registrations for huzcodes identity extension services.
        /// </summary>
        /// <param name="SigningKey">The Key that used for Signing the token and passed here to validate it.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthZ(this IServiceCollection services, string SigningKey)
        {
            // adding registrations for huzcodes identity extension services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<JwtSecurityTokenHandler>();
            services.AddScoped<IIdentityManager, IdentityServices>();

            // adding registration for jwt token authorizations
            var oKey = Encoding.ASCII.GetBytes(SigningKey);
            var oKeySym256 = new SymmetricSecurityKey(oKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = HuzcodesJwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = HuzcodesJwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = HuzcodesJwtBearerDefaults.AuthenticationScheme;
            }).AddScheme<AuthenticationSchemeOptions, HuzcodesAuthZHandler>(HuzcodesJwtBearerDefaults.AuthenticationScheme, options => { })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuerSigningKey = true,
                      IssuerSigningKey = oKeySym256,
                      ValidateLifetime = true,
                      ValidateIssuer = false,
                      ValidateAudience = false
                  };

                  options.Events = new JwtBearerEvents
                  {
                      OnMessageReceived = context =>
                      {
                          var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                          if (string.IsNullOrEmpty(token))
                          {
                              context.NoResult();
                          }
                          return Task.CompletedTask;
                      },
                      OnAuthenticationFailed = context =>
                      {
                          context.NoResult();
                          return Task.CompletedTask;
                      }
                  };
              });

            return services;
        }


        /// <summary>
        /// Registring a middleware for Identity API Key AuthZ for other Apis access.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddAuthZMiddleWare(this IApplicationBuilder app)
        {
            app.UseMiddleware<IdentityMiddleware>();
            return app;
        }
    }
}
