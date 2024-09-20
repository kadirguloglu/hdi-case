using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class AggrementService : IAggrementService
{
    private readonly IDatabaseContext<Aggrement> _context;
    private readonly ICompanyService _companyService;
    private readonly IStorageService<Aggrement> _storage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDatabaseContext<AggrementFile> _aggrementFileContext;
    private readonly ILogger<AggrementService> _logger;
    private readonly INotificationHubDispatcher _notificationHubDispatcher;
    public AggrementService(
        IDatabaseContext<Aggrement> context,
        ICompanyService companyService,
        IStorageService<Aggrement> storage,
        IHttpContextAccessor httpContextAccessor,
        IDatabaseContext<AggrementFile> aggrementFileContext,
        ILogger<AggrementService> logger,
        INotificationHubDispatcher notificationHubDispatcher
    )
    {
        _context = context;
        _companyService = companyService;
        _storage = storage;
        _httpContextAccessor = httpContextAccessor;
        _aggrementFileContext = aggrementFileContext;
        _logger = logger;
        _notificationHubDispatcher = notificationHubDispatcher;
    }

    public async Task<Result<int>> AddNewAggrement(AddNewAggrementRequest model)
    {
        var company = await _companyService.GetCompanyWithHeaderApiKey();
        if (!company.IsSuccessfull || company.Data is null)
        {
            return new Result<int>(false, company.Message);
        }
        var aggrement = new Aggrement
        {
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            CompanyId = company.Data.Id,
        };
        var result = await _context.InsertAsync(aggrement);
        if (result)
        {
            try
            {
                IFormFileCollection? files = _httpContextAccessor.HttpContext?.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    List<AggrementFile> aggrementFiles = new List<AggrementFile>();
                    foreach (var item in files)
                    {
                        var storageResult = await _storage.Upload(aggrement.Id, item);
                        if (storageResult != null)
                        {
                            aggrementFiles.Add(new AggrementFile
                            {
                                AggrementId = aggrement.Id,
                                FileName = storageResult.FileName,
                                FilePath = storageResult.FilePath,
                                FileSize = storageResult.FileSize,
                                FileText = storageResult.FileText,
                                CreatedDate = DateTime.Now,
                                LastUpdatedDate = DateTime.Now
                            });
                        }
                    }
                    if (aggrementFiles.Count > 0)
                    {
                        await _aggrementFileContext.InsertManyAsync(aggrementFiles);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Aggrement file builder error");
            }
            await _notificationHubDispatcher.NewAggrementNotification(aggrement.Id);
            return new Result<int>(true, aggrement.Id);
        }
        return new Result<int>(false);
    }
}