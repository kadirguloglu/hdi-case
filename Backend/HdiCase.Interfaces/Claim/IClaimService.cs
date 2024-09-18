public interface IClaimService
{
    T? GetClaimValue<T>(Enum_UserClaims claim);
    int? GetUserId();
    string? GetDeviceCode();
}
