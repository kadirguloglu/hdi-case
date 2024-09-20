using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.Mvc;

namespace HdiCase.RestApi.Controllers.OData.v1;
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService RoleService)
    {
        _roleService = RoleService;
    }

    [PermissionAuthorization(Enum_Permission.RoleList)]
    [EnableQuery]
    public IQueryable<Role> Get()
    {
        var result = _roleService.Queryable();
        return result;
    }

    [PermissionAuthorization(Enum_Permission.RoleList)]
    public IActionResult Get([FromODataUri] int key)
    {
        return Ok(_roleService.Queryable().FirstOrDefault(x => x.Id == key));
    }

    [PermissionAuthorization(Enum_Permission.RoleInsert)]
    [EnableQuery]
    public async Task<IActionResult> Post([FromBody] Role model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _roleService.AddRole(model);
        return Ok(result);
    }

    [PermissionAuthorization(Enum_Permission.RoleUpdate)]
    [EnableQuery]
    public async Task<IActionResult> Patch([FromODataUri] int key, [FromBody] Delta<Role> model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await _roleService.UpdateRole(key, model);
        return Ok(result);
    }

    [PermissionAuthorization(Enum_Permission.RoleDelete)]
    [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
    public async Task<IActionResult> Delete([FromODataUri] int key)
    {
        var result = await _roleService.DeleteRole(key);
        return Ok(result);
    }

}