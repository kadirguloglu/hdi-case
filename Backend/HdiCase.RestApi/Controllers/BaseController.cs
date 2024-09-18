using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HdiCase.RestApi.Controllers;
[ApiVersion("1.0")]
[Tags(new[] { "test" })]
[ApiExplorerSettings(GroupName = "Api-v1")]
[Authorize(AuthenticationSchemes = "UserJwt")]
public class BaseController : ControllerBase
{

}
