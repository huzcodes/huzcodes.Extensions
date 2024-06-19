using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace huzcodes.Extensions.Identity
{
    public static class IdentityRegistrations
    {
        /// <summary>
        /// Adding registration for Authorization for JWT Bearer
        /// </summary>
        /// <param name="SigningKey">The Key that used for Signing the token and passed here to validate it.</param>
        /// <returns></returns>
        public static IServiceCollection AddAuthZ(this IServiceCollection services, string SigningKey) 
        {
            var oKey = Encoding.ASCII.GetBytes(SigningKey);
            var oKeySym256 = new SymmetricSecurityKey(oKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
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
            });

            return services;
        }


        /// <summary>
        /// Registring a middleware for Identity API Key AuthZ for other Apis access.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddIdentityAPIKeyMiddleWare(this IApplicationBuilder app) 
        {
            app.UseMiddleware<IdentityKeyMiddleware>();
            return app;
        }
    }
}
