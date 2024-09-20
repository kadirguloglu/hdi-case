using LQ = System.Linq.Expressions;

public interface IDatabaseContext<TCollection>
    where TCollection : class, ITableEntities
{
    IQueryable<TCollection> Queryable();
    Task<bool> InsertAsync(TCollection entity);
    Task<bool> InsertManyAsync(List<TCollection> entity);
    //var result = await _context.UpdateAsync(x => x.Id == pollId, Builders<Polls>.Update.Set(x => x.IsPublish, true));
    Task<bool> UpdateAsync(TCollection entity);
    Task<TCollection?> GetByIdAsync(int id);
    Task<List<TCollection>?> GetByIdsAsync(int[] ids);
    Task<List<TCollection>?> GetAsync(LQ.Expression<Func<TCollection, bool>> predicate);
    Task<List<TCollection>?> GetAsync<TKey>(
        LQ.Expression<Func<TCollection, bool>> predicate,
        LQ.Expression<Func<TCollection, TKey>> orderByPredicate,
        bool isDescending = false);
    Task<List<TCollection>?> GetAsync<TKey>(
        LQ.Expression<Func<TCollection, bool>> predicate,
        LQ.Expression<Func<TCollection, TKey>> orderByPredicate,
        int limit,
        bool isDescending = false);
    Task<List<TCollection>?> GetAsync<TKey>(
        LQ.Expression<Func<TCollection, bool>> predicate,
        LQ.Expression<Func<TCollection, TKey>> orderByPredicate,
        int page,
        int pageSize,
        bool isDescending = false);
    Task<TCollection?> FirstAsync(LQ.Expression<Func<TCollection, bool>> predicate);
    Task<bool?> AnyAsync(LQ.Expression<Func<TCollection, bool>> predicate);
    Task<bool> DeleteAsync(LQ.Expression<Func<TCollection, bool>> predicate);
    Task<bool> DeleteByIdAsync(int id);
}