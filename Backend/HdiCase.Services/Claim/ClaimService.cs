using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class ClaimService : IClaimService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ClaimService(
        IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public T? GetClaimValue<T>(Enum_UserClaims claim)
    {
        var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        var claimValue = identity?.FindFirst(claim.ToString())?.Value;

        if (claimValue != null)
        {
            try
            {
                return (T)Convert.ChangeType(claimValue, typeof(T));
            }
            catch
            {
                // Dönüştürme hatası, uygun bir log eklemeyi düşünebilirsiniz.
                return default;
            }
        }

        return default;
    }

    public int? GetUserId()
    {
        return GetClaimValue<int?>(Enum_UserClaims.Id);
    }

    public string? GetDeviceCode()
    {
        return GetClaimValue<String>(Enum_UserClaims.DeviceCode);
    }
}