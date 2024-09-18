using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HdiCase.RestApi.Controllers.Api.v1;

[ApiController]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IAuthenticationService authenticationService
    )
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<Result<AuthenticationResponse>> Login(AuthenticationRequest model)
    {
        var result = await _authenticationService.Login(model);
        return result;
    }

    [HttpGet("GetCurrentUser")]
    public async Task<Result<GetCurrentUserResponse>> GetCurrentUser()
    {
        return await _authenticationService.GetCurrentUser();
    }
}