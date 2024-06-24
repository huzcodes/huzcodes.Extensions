namespace huzcodes.Extensions.Identity
{
    public interface IIdentityManager
    {
        /// <summary>
        /// This function is used for generating jwt token by providing some parameters.
        /// </summary>
        /// <typeparam name="TClaims">Generic class the holds the claims properties.</typeparam>
        /// <param name="claimsName">The name for JwtRegisteredClaimNames and the value is the claims content.</param>
        /// <param name="signingKey">The key that used for signing the token, while generating it.</param>
        /// <param name="expiresAt">How long token is valid.</param>
        /// <param name="subject">Claim subject for the token, optional.</param>
        /// <returns></returns>
        string GenerateJwtToken<TClaims>(TClaims claims,
                                         string signingKey,
                                         DateTime expiresAt,
                                         string claimsName = "data",
                                         string subject = "");

        /// <summary>
        /// Decoding the token from the http context header authorization,
        /// and read the claims out of based on the type of claims that provided while generating the token.
        /// </summary>
        /// <typeparam name="TClaims">Generic class the holds the claims properties.</typeparam>
        /// <param name="claimsName">The name for JwtRegisteredClaimNames and the value is the claims content.</param>
        /// <returns></returns>
        TClaims DecodeToken<TClaims>(string claimsName = "data");

        /// <summary>
        /// Decoding the token from the provided argument jwt token,
        /// and read the claims out of based on the type of claims that provided while generating the token.
        /// </summary>
        /// <typeparam name="TClaims">Generic class the holds the claims properties.</typeparam>
        /// <param name="jwtToken">The jwt token itself</param>
        /// <param name="claimsName">The name for JwtRegisteredClaimNames and the value is the claims content.</param>
        /// <returns></returns>
        TClaims DecodeToken<TClaims>(string jwtToken, string claimsName = "data");
    }
}
