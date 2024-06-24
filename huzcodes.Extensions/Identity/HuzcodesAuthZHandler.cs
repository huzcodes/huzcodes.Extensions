using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.DependencyInjection;

namespace huzcodes.Extensions.Identity
{
    public class HuzcodesAuthZHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public HuzcodesAuthZHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check for API Key
            if (Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
            {
                var appSettings = Context.RequestServices.GetRequiredService<IConfiguration>();
                var validApiKey = appSettings.GetValue<string>("X-Api-Key");

                if (apiKey == validApiKey)
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, "ApiKeyUser") };
                    var identity = new ClaimsIdentity(claims, "ApiKey");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }

            // Fallback to JWT Bearer
            var result = await Context.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                return AuthenticateResult.Success(result.Ticket);
            }

            return AuthenticateResult.Fail("Unauthorized");
        }
    }
}
