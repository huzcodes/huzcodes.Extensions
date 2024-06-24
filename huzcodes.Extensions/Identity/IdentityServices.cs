using huzcodes.Extensions.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace huzcodes.Extensions.Identity
{
    public class IdentityServices(IHttpContextAccessor contextAccessor) : IIdentityManager
    {
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        /// <summary>
        /// This function is used for generating jwt token by providing some parameters.
        /// </summary>
        /// <typeparam name="TClaims">Generic class the holds the claims properties.</typeparam>
        /// <param name="claimsName">The name for JwtRegisteredClaimNames and the value is the claims content.</param>
        /// <param name="signingKey">The key that used for signing the token, while generating it.</param>
        /// <param name="expiresAt">How long token is valid.</param>
        /// <param name="subject">Claim subject for the token, optional.</param>
        /// <returns></returns>
        public string GenerateJwtToken<TClaims>(TClaims claims,
                                                string signingKey,
                                                DateTime expiresAt,
                                                string claimsName = "data",
                                                string subject = "")
        {
            if (claims == null)
                return string.Empty;

            var oJwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var oSigningKey = Encoding.ASCII.GetBytes(signingKey);
            if (signingKey.Count() < 30)
                throw new ResultException("Sha256 signing key size must be greater than: '256' bits, so you must key with length of 44 or more character.",
                                          (int)HttpStatusCode.Forbidden);

            var oSecurityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(JwtRegisteredClaimNames.Sub, subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(claimsName, JsonConvert.SerializeObject(claims))
                ]),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(oSigningKey),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };
            var token = oJwtSecurityTokenHandler.CreateToken(oSecurityTokenDescriptor);
            return oJwtSecurityTokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Decoding the token from the http context header authorization,
        /// and read the claims out of based on the type of claims that provided while generating the token.
        /// </summary>
        /// <typeparam name="TClaims">Generic class the holds the claims properties.</typeparam>
        /// <param name="claimsName">The name for JwtRegisteredClaimNames and the value is the claims content.</param>
        /// <returns></returns>
        public TClaims DecodeToken<TClaims>(string claimsName = "data")
        {
            if (_contextAccessor == null || 
                _contextAccessor.HttpContext == null || 
                _contextAccessor.HttpContext.User == null)
                return default!;

            var oClaims = _contextAccessor.HttpContext.User.Claims.Where(options => options.Type == claimsName)
                                                                  .Select(options => options.Value);
            if(!oClaims.Any())
                return default!;

            var oClaimsContent = JsonConvert.DeserializeObject<TClaims>(oClaims.FirstOrDefault()!);

            return oClaimsContent!;
        }

        /// <summary>
        /// Decoding the token from the provided argument jwt token,
        /// and read the claims out of based on the type of claims that provided while generating the token.
        /// </summary>
        /// <typeparam name="TClaims">Generic class the holds the claims properties.</typeparam>
        /// <param name="jwtToken">The jwt token itself</param>
        /// <param name="claimsName">The name for JwtRegisteredClaimNames and the value is the claims content.</param>
        /// <returns></returns>
        public TClaims DecodeToken<TClaims>(string jwtToken, string claimsName = "data")
        {
            var oJwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            if (string.IsNullOrEmpty(jwtToken))
                return default!;

            if (jwtToken.Contains("Bearer"))
                jwtToken = jwtToken.Replace("Bearer", " ").TrimStart();
            
            var jwtTokenContent = oJwtSecurityTokenHandler.ReadJwtToken(jwtToken);

            if (!jwtTokenContent.Claims.Any())
                return default!;

            var oClaims = jwtTokenContent.Claims.Where(options => options.Type == claimsName)
                                                .Select(options => options.Value);

            var oClaimsContent = JsonConvert.DeserializeObject<TClaims>(oClaims.FirstOrDefault()!);

            return oClaimsContent!;
        }

    }
}
