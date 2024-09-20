using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Polly;

public class NotificationHubDispatcher : INotificationHubDispatcher
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IDatabaseContext<Aggrement> _aggrementContext;
    private readonly IDatabaseContext<Company> _companyContext;
    private readonly ILogger<NotificationHubDispatcher> _logger;
    public NotificationHubDispatcher(
        IHubContext<NotificationHub> hubContext,
        IDatabaseContext<Aggrement> aggrementContext,
        IDatabaseContext<Company> companyContext,
        ILogger<NotificationHubDispatcher> logger
    )
    {
        _hubContext = hubContext;
        _aggrementContext = aggrementContext;
        _companyContext = companyContext;
        _logger = logger;
    }

    public async Task<Result<bool>> NewAggrementNotification(int aggrementId)
    {
        // polly ile bir hata durumunda bildirimin 3 kere atmayi denemesini sagliyoruz.
        var retryPolicy = Policy
            .Handle<Exception>()
            .RetryAsync(3, (exception, retryCount) =>
            {
                _logger.LogError(exception, $"Attempt {retryCount} failed in NewAggrementNotification.");
            });
        return await retryPolicy.ExecuteAsync(async () =>
        {
            try
            {
                var aggrement = await _aggrementContext.GetByIdAsync(aggrementId);
                if (aggrement is null)
                {
                    return new Result<bool>(false);
                }
                var company = await _companyContext.GetByIdAsync(aggrement.CompanyId);
                if (company is null)
                {
                    return new Result<bool>(false);
                }
                await _hubContext.Clients.All.SendAsync("NewAggrementNotification", new
                {
                    Company = company,
                    Aggrement = aggrement
                });
                return new Result<bool>(true);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "NewAggrementNotification");
            }
            return new Result<bool>(false);
        });
    }
}