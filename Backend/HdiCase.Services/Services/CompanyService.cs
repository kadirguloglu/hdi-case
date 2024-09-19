using Microsoft.AspNetCore.Http;

public class CompanyService : ICompanyService
{
    private readonly IDatabaseContext<Company> _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CompanyService(
        IDatabaseContext<Company> context,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<Company>> GetCompanyWithHeaderApiKey()
    {
        var apiKey = _httpContextAccessor.HttpContext?.Request.Headers["X-Api-Key"].ToString();
        if (string.IsNullOrEmpty(apiKey))
        {
            return new Result<Company>(false, Enum_ResourceKeys.ApiKeyNotFound.ToString());
        }
        var company = await _context.FirstAsync(x => x.ApiKey == apiKey);
        if (company is null)
        {
            return new Result<Company>(false, Enum_ResourceKeys.CompanyNotFound.ToString());
        }
        if (!company.ApiIsActive)
        {
            return new Result<Company>(false, Enum_ResourceKeys.CompanyNotFound.ToString());
        }
        return new Result<Company>(true, company);
    }
}