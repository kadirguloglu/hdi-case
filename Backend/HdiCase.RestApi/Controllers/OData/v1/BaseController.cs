using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace HdiCase.RestApi.Controllers.OData.v1
{
    [ApiVersion("1.0")]
    [Tags(new[] { "OData-v1" })]
    [Authorize(AuthenticationSchemes = "UserJwt")]
    public class BaseController : ODataController
    {
    }
}
