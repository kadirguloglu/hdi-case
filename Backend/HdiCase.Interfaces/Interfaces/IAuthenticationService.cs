
public interface IAuthenticationService
{
    Task<Result<AuthenticationResponse>> Login(AuthenticationRequest model);
    Task<Result<GetCurrentUserResponse>> GetCurrentUser();
}