using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;

namespace HdiCase.RestApi.Controllers.OData.v1;
public class LoggingController : BaseController
{
    private readonly ILoggingService _service;
    public LoggingController(
        ILoggingService service
    )
    {
        _service = service;
    }

    [PermissionAuthorization(Enum_Permission.LoggingList)]
    [EnableQuery]
    public IQueryable<Logging> Get()
    {
        var result = _service.Queryable();
        return result;
    }

    [PermissionAuthorization(Enum_Permission.LoggingList)]
    public IActionResult Get([FromODataUri] int key)
    {
        return Ok(_service.Queryable().FirstOrDefault(x => x.Id == key));
    }
}