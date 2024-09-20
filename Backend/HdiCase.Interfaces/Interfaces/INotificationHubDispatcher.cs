public interface INotificationHubDispatcher
{
    Task<Result<bool>> NewAggrementNotification(int aggrementId);
}