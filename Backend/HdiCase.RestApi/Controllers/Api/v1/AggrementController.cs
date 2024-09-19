
using HdiCase.RestApi.Controllers.Api.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

[ApiController]
[EnableRateLimiting("ApiKeyPolicy")]
public class AggrementController : BaseController
{
    private readonly IAggrementService _service;
    public AggrementController(
        IAggrementService service
    )
    {
        _service = service;
    }

    [HttpPost("AddNewAggrement")]
    [AllowAnonymous]
    public async Task<Result<int>> AddNewAggrement([FromForm] AddNewAggrementRequest model)
    {
        return await _service.AddNewAggrement(model);
    }
}