public interface ICompanyService
{
    Task<Result<Company>> GetCompanyWithHeaderApiKey();
}