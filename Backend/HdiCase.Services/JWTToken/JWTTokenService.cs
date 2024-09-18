using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

public class JWTTokenService : IJWTTokenService
{
    private readonly ILogger<JWTTokenService> _logger;
    public JWTTokenService(
        ILogger<JWTTokenService> logger
    )
    {
        _logger = logger;
    }

    public GenerateAccessTokenResponse GenerateAdminAccessToken(AdminLoginData user)
    {
        SecurityToken token;
        var claims = new[] {
                            new Claim(Enum_UserClaims.Id.ToString(), user.Id.ToString() ?? ""),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };
        var key = Encoding.UTF8.GetBytes(EnvironmentSettings.JWTSecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = EnvironmentSettings.JWTValidIssuer,
            Audience = EnvironmentSettings.JWTValidAudience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(EnvironmentSettings.JWTExpiresIn),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        token = tokenHandler.CreateToken(tokenDescriptor);
        var bearerToken = tokenHandler.WriteToken(token);
        return new GenerateAccessTokenResponse()
        {
            BearerToken = bearerToken,
        };
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        try
        {
            var key = Encoding.UTF8.GetBytes(EnvironmentSettings.JWTSecret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = EnvironmentSettings.JWTValidIssuer,
                ValidAudience = EnvironmentSettings.JWTValidAudience,
                ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while validating token");
            return null;
        }
    }
}