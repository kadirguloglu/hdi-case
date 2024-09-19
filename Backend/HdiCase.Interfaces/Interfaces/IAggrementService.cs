public interface IAggrementService
{
    Task<Result<int>> AddNewAggrement(AddNewAggrementRequest model);
}