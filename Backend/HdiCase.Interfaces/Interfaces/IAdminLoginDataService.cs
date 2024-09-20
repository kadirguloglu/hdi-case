using Microsoft.AspNetCore.OData.Deltas;

public interface IAdminLoginDataService
{
    IQueryable<AdminLoginData> Queryable();
    Task<Result<bool>> AddAdminLoginData(AdminLoginData model);
    Task<Result<bool>> UpdateAdminLoginData(int id, Delta<AdminLoginData> model);
    Task<Result<bool>> UpdateAdminLoginData(AdminLoginData model);
    Task<Result<bool>> DeleteAdminLoginData(int id);
}