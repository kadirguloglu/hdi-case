
using BN = BCrypt.Net;

public class AuthenticationService : IAuthenticationService
{
    private readonly IDatabaseContext<AdminLoginData> _adminLoginDataContext;
    private readonly IDatabaseContext<Role> _roleContext;
    private readonly IJWTTokenService _jwtTokenService;
    private readonly IClaimService _claimService;
    public AuthenticationService(
        IDatabaseContext<AdminLoginData> adminLoginDataContext,
        IJWTTokenService jwtTokenService,
        IDatabaseContext<Role> roleContext,
        IClaimService claimService)
    {
        _adminLoginDataContext = adminLoginDataContext;
        _jwtTokenService = jwtTokenService;
        _roleContext = roleContext;
        _claimService = claimService;
    }

    public async Task<Result<AuthenticationResponse>> Login(AuthenticationRequest model)
    {
        AdminLoginData? usr = null;
        if (!string.IsNullOrEmpty(model.Email))
        {
            usr = await _adminLoginDataContext.FirstAsync(x => x.Email == model.Email);
        }
        if (usr is null)
        {
            return new Result<AuthenticationResponse>(false, Enum_ResourceKeys.EmailOrUserNameOrPasswordIsNotValid.ToString());
        }
        if (!usr.IsActive)
        {
            return new Result<AuthenticationResponse>(false, Enum_ResourceKeys.UserIsDisabled.ToString());
        }
        if (!BN.BCrypt.Verify(model.Password, usr.Password))
        {
            return new Result<AuthenticationResponse>(false, Enum_ResourceKeys.EmailOrUserNameOrPasswordIsNotValid.ToString());
        }
        await _adminLoginDataContext.UpdateAsync(usr);
        var bearerToken = _jwtTokenService.GenerateAdminAccessToken(usr);
        return new Result<AuthenticationResponse>(true, new AuthenticationResponse()
        {
            BearerToken = bearerToken.BearerToken,
            UserId = usr.Id
        }
        );
    }

    public async Task<Result<GetCurrentUserResponse>> GetCurrentUser()
    {
        var currentUserId = _claimService.GetUserId();
        if (currentUserId == null)
        {
            return new Result<GetCurrentUserResponse>(false);
        }
        var user = await _adminLoginDataContext.GetByIdAsync(currentUserId.Value);
        if (user == null)
        {
            return new Result<GetCurrentUserResponse>(false);
        }
        if (!user.IsActive)
        {
            return new Result<GetCurrentUserResponse>(false);
        }
        var roles = new List<Role>();
        if (user.RoleId?.Count() > 0)
        {
            roles = (await _roleContext.GetAsync(x => user.RoleId.Contains(x.Id))) ?? new List<Role>();
        }
        return new Result<GetCurrentUserResponse>(true, new GetCurrentUserResponse
        {
            Roles = roles,
            AdminLoginData = user
        });
    }
}