using HdiCase.RestApi.Controllers.Api.v1;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("UpdateRole")]
    public async Task<Result<bool>> UpdateRole(RoleDto role)
    {
        var result = await _roleService.UpdateRole(role);
        return result;
    }

    [HttpGet("GetRoleById/{id}")]
    public async Task<Result<RoleDto>> GetRoleById(int id)
    {
        var result = await _roleService.GetRoleById(id);
        return result;
    }
}