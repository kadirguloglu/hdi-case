using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;

namespace HdiCase.RestApi.Controllers.OData.v1;
public class AdminLoginDataController : BaseController
{
    private readonly IAdminLoginDataService _adminLoginDataService;
    public AdminLoginDataController(IAdminLoginDataService AdminLoginDataService)
    {
        _adminLoginDataService = AdminLoginDataService;
    }

    [PermissionAuthorization(Enum_Permission.AdminLoginDataList)]
    [EnableQuery]
    public IQueryable<AdminLoginData> Get()
    {
        var result = _adminLoginDataService.Queryable();
        return result;
    }

    [PermissionAuthorization(Enum_Permission.AdminLoginDataList)]
    public IActionResult Get([FromODataUri] int key)
    {
        return Ok(_adminLoginDataService.Queryable().FirstOrDefault(x => x.Id == key));
    }

    [PermissionAuthorization(Enum_Permission.AdminLoginDataInsert)]
    [EnableQuery]
    public async Task<IActionResult> Post([FromBody] AdminLoginData model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _adminLoginDataService.AddAdminLoginData(model);
        return Ok(result);
    }

    [PermissionAuthorization(Enum_Permission.AdminLoginDataUpdate)]
    [EnableQuery]
    public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<AdminLoginData> model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _adminLoginDataService.UpdateAdminLoginData(key, model);
        return Ok(result);
    }

    [PermissionAuthorization(Enum_Permission.AdminLoginDataDelete)]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        var result = await _adminLoginDataService.DeleteAdminLoginData(key);
        return Ok(result);
    }

}