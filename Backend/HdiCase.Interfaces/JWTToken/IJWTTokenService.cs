using System.Security.Claims;

public interface IJWTTokenService
{
    GenerateAccessTokenResponse GenerateAdminAccessToken(AdminLoginData user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}