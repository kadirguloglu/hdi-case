using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HdiCase.RestApi.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    [Tags(new[] { "Api-v1" })]
    [ApiExplorerSettings(GroupName = "Api-v1")]
    [RouteKey(Enum_RoutingKeys.Api, "api/v1/[controller]")]
    [Authorize(AuthenticationSchemes = "UserJwt")]
    public class BaseController : ControllerBase
    {
    }
}
