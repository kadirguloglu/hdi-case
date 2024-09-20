using Microsoft.AspNetCore.OData.Deltas;
using BN = BCrypt.Net;

public class AdminLoginDataService : IAdminLoginDataService
{
    private readonly IDatabaseContext<AdminLoginData> _context;
    public AdminLoginDataService(IDatabaseContext<AdminLoginData> context)
    {
        _context = context;
    }

    public IQueryable<AdminLoginData> Queryable()
    {
        var result = _context.Queryable();
        return result;
    }

    public async Task<Result<bool>> AddAdminLoginData(AdminLoginData model)
    {
        model.Password = BN.BCrypt.HashPassword(model.Password);
        var result = await _context.InsertAsync(model);
        return new Result<bool>(result, result);
    }

    public async Task<Result<bool>> UpdateAdminLoginData(int id, Delta<AdminLoginData> model)
    {
        var oldModel = await _context.GetByIdAsync(id);
        if (oldModel is null) return new Result<bool>(false, false);
        bool hashPassword = false;
        if (model.TryGetPropertyValue("Password", out var password))
        {
            if (password.ToString() != oldModel.Password)
            {
                hashPassword = true;
            }
        }
        model.Patch(oldModel);
        if (hashPassword)
        {
            oldModel.Password = BN.BCrypt.HashPassword(oldModel.Password);
        }
        var result = await _context.UpdateAsync(oldModel);
        return new Result<bool>(result, result);
    }

    public async Task<Result<bool>> UpdateAdminLoginData(AdminLoginData model)
    {
        var result = await _context.UpdateAsync(model);
        return new Result<bool>(result, result);
    }

    public async Task<Result<bool>> DeleteAdminLoginData(int id)
    {
        var data = await _context.GetByIdAsync(id);
        if (data is null)
        {
            return new Result<bool>(false, false);
        }
        if (data.IsDeveloper)
        {
            return new Result<bool>(false, false);
        }
        var result = await _context.DeleteByIdAsync(id);
        return new Result<bool>(result, result);
    }
}