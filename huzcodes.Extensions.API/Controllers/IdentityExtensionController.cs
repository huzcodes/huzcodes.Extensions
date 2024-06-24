using huzcodes.Extensions.API.Models;
using huzcodes.Extensions.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace huzcodes.Extensions.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IdentityExtensionController(IIdentityManager identityManager,
                                             IHttpContextAccessor httpContextAccessor,
                                             IConfiguration configuration) : ControllerBase
    {
        private readonly IIdentityManager _identityManager = identityManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IConfiguration _configuration = configuration;

        [AllowAnonymous]
        [HttpGet]
        [Route("/generateJwtToken")]
        public IActionResult Get()
        {
            var oIdentity = new IdentityModel()
            {
                Email = "huz@huzcodes.com",
                FirstName = "huz",
                LastName = "codes"
            };
            var oSigningKey = _configuration["SigningKey"];
            var oToken = _identityManager.GenerateJwtToken(oIdentity,
                                                           oSigningKey!,
                                                           DateTime.UtcNow.AddHours(1));
            return Ok(oToken);
        }

        [Authorize]
        [HttpGet]
        [Route("/decodeJwtTokenContent")]
        public IActionResult TokenContentDecodeFunc()
        {
            var oToken = _identityManager.DecodeToken<IdentityModel>();
            return Ok(oToken);
        }

        [Authorize]
        [HttpGet]
        [Route("/decodeJwtTokenContentByToken")]
        public IActionResult TokenContentByJwtToken()
        {
            var oJwtToken = _httpContextAccessor.GetAuthorizationHeader();
            var oToken = _identityManager.DecodeToken<IdentityModel>(jwtToken: oJwtToken);
            return Ok(oToken);
        }

        [Authorize]
        [HttpGet]
        [Route("/validatingWithApiKey")]
        public IActionResult ValidatingWithApiKey()
        {
            var oIdentity = new IdentityModel()
            {
                Email = "huz@huzcodes.com",
                FirstName = "huz",
                LastName = "codes"
            };
            return Ok(oIdentity);
        }
    }
}
