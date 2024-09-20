using Microsoft.AspNetCore.OData.Deltas;

public interface IRoleService
{
    IQueryable<Role> Queryable();
    Task<Result<bool>> AddRole(Role model);
    Task<Result<bool>> UpdateRole(RoleDto model);
    Task<Result<bool>> UpdateRole(int id, Delta<Role> model);
    Task<Result<bool>> DeleteRole(int id);
    Task<Result<RoleDto>> GetRoleById(int id);
}