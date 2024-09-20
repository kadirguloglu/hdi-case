using Microsoft.AspNetCore.OData.Deltas;

public class RoleService : IRoleService
{
    private readonly IDatabaseContext<Role> _context;
    private readonly IDatabaseContext<AdminLoginData> _adminLoginDataContext;
    private readonly IAdminLoginDataService _adminLoginDataService;

    public RoleService(IDatabaseContext<Role> context, IDatabaseContext<AdminLoginData> adminLoginDataContext, IAdminLoginDataService adminLoginDataService)
    {
        _context = context;
        _adminLoginDataContext = adminLoginDataContext;
        _adminLoginDataService = adminLoginDataService;
    }

    public IQueryable<Role> Queryable()
    {
        return _context.Queryable();
    }

    public async Task<Result<bool>> AddRole(Role model)
    {
        var result = await _context.InsertAsync(model);
        return new Result<bool>(result, result);
    }

    public async Task<Result<bool>> UpdateRole(RoleDto model)
    {
        if (model.Id < 0) return new Result<bool>(false, false, "RoleId information can not be empty");
        Role? role = await _context.GetByIdAsync(model.Id);
        if (role is null) return new Result<bool>(false, false, "Can not find role by role id");
        var result = await _context.UpdateAsync(model);
        if (result)
        {
            List<int> roleNewAdmins = new List<int>();
            if (model.Admins != null) roleNewAdmins = model.Admins;
            var admins = await _adminLoginDataContext.GetAsync(x => x.RoleId != null && x.RoleId.Contains(role.Id));
            if (admins != null)
            {
                foreach (AdminLoginData ald in admins)
                {
                    if (roleNewAdmins.Contains(ald.Id))
                    {
                        roleNewAdmins.Remove(ald.Id);
                    }
                    else
                    {
                        List<int> roleIds = ald.RoleId?.ToList() ?? new List<int>();
                        roleIds.Remove(role.Id);
                        ald.RoleId = roleIds.ToArray();
                        await _adminLoginDataService.UpdateAdminLoginData(ald);
                    }
                }
            }
            foreach (int adminId in roleNewAdmins)
            {
                AdminLoginData? admin = await _adminLoginDataContext.GetByIdAsync(adminId);
                if (admin != null)
                {
                    List<int> roleIds = new List<int>();
                    if (admin.RoleId != null) roleIds = admin.RoleId.ToList();
                    roleIds.Add(role.Id);
                    admin.RoleId = roleIds.ToArray();
                    await _adminLoginDataService.UpdateAdminLoginData(admin);
                }
            }
        }
        return new Result<bool>(result, result);
    }

    public async Task<Result<bool>> UpdateRole(int id, Delta<Role> model)
    {
        var oldModel = await _context.GetByIdAsync(id);
        if (oldModel is null)
        {
            return new Result<bool>(false, false);
        }
        model.Patch(oldModel);
        var result = await _context.UpdateAsync(oldModel);
        return new Result<bool>(result, result);
    }

    public async Task<Result<bool>> DeleteRole(int id)
    {
        var result = await _context.DeleteByIdAsync(id);
        if (result)
        {
            List<AdminLoginData>? adminsResult = await _adminLoginDataContext.GetAsync(x => x.RoleId != null && x.RoleId.Contains(id));
            if (adminsResult != null && adminsResult.Any())
            {
                foreach (var admin in adminsResult)
                {
                    if (admin.RoleId != null)
                    {
                        List<int> roles = admin.RoleId.ToList();
                        roles.Remove(id);
                        admin.RoleId = roles.ToArray();
                        await _adminLoginDataService.UpdateAdminLoginData(admin);
                    }
                }
            }
        }
        return new Result<bool>(result, result);
    }

    public async Task<Result<RoleDto>> GetRoleById(int id)
    {
        Role? role = await _context.GetByIdAsync(id);
        if (role != null)
        {
            RoleDto dto = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description,
                PermissionKeys = role.PermissionKeys
            };
            List<AdminLoginData>? admins = await _adminLoginDataContext.GetAsync(x => x.RoleId != null && x.RoleId.Contains(role.Id));
            if (admins != null)
            {
                var roles = admins.Select(x => x.Id).Where(x => x > 0).ToList();
                dto.Admins = roles;
            }
            return new Result<RoleDto>(true, dto);
        }
        return new Result<RoleDto>(false);
    }

}