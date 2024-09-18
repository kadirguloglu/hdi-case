public interface IClaimService
{
    T? GetClaimValue<T>(Enum_UserClaims claim);
    string? GetUserId();
    string? GetDeviceCode();
}
