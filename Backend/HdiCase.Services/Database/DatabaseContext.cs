using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LQ = System.Linq.Expressions;

public class DatabaseContext<TCollection> : IDatabaseContext<TCollection>
    where TCollection : class, ITableEntities
{
    private readonly HdiDbContext _context;
    private readonly IRedisCacheService _distributedCache;
    private readonly DbSet<TCollection> _collectionContext;
    private readonly ILogger<DatabaseContext<TCollection>> _logger;
    private readonly TCollection collection;
    public IDatabaseContext<Logging>? _logging { get; set; }
    private readonly IClaimService? _claimService;
    private readonly IHttpContextAccessor? _httpContextAccessor;
    private readonly TimeSpan _cacheTime = TimeSpan.FromHours(1);
    public DatabaseContext(
        HdiDbContext context,
        ILogger<DatabaseContext<TCollection>> logger,
        IRedisCacheService distributedCache,
        IClaimService claimService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _context = context;
        _collectionContext = context.Set<TCollection>();
        var type = typeof(TCollection);
        collection = Activator.CreateInstance<TCollection>();
        _logger = logger;
        _distributedCache = distributedCache;
        if (typeof(TCollection) != typeof(Logging) && httpContextAccessor?.HttpContext != null)
        {
            _logging = httpContextAccessor.HttpContext.RequestServices.GetService<IDatabaseContext<Logging>>() as IDatabaseContext<Logging>;
        }
        _claimService = claimService;
        if (httpContextAccessor != null)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }

    public string? ClientIpAddress
    {
        get
        {
            var context = _httpContextAccessor?.HttpContext;
            string? clientIp = context?.Connection?.RemoteIpAddress?.ToString();
            if (context?.Request.Headers is not null && context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            return clientIp;
        }
    }

    private async Task AddLog(TCollection? newData, TCollection? oldData, Enum_OperationType operationType)
    {
        if (EnvironmentSettings.LoggingIsEnabled)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters =
                {
                    new IgnoreUnsupportedTypesConverter()
                }
            };
            if (_logging != null)
            {
                await _logging.InsertAsync(new Logging
                {
                    IpAddress = ClientIpAddress,
                    NewData = JsonSerializer.Serialize(newData, options),
                    OldData = JsonSerializer.Serialize(oldData, options),
                    OperationType = operationType,
                    TableId = newData?.Id ?? null,
                    TableName = collection.CollectionName,
                    UserId = _claimService?.GetUserId()
                });
            }
        }
    }

    private async Task RemoveCache()
    {
        var type = typeof(TCollection).Name;
        await Task.CompletedTask;
    }

    public async Task<bool> InsertAsync(TCollection entity)
    {
        try
        {
            entity.CreatedDate = DateTime.UtcNow;
            entity.LastUpdatedDate = DateTime.UtcNow;
            var test = await _collectionContext.AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            await _distributedCache.SetAsync(Enum_RedisCacheKeys.ModelCaching, entity, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = _cacheTime
            }, collection.CollectionName, entity.Id.ToString() ?? "ModelIdNotFound");
            await AddLog(entity, entity, Enum_OperationType.Insert);
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            _logger.LogError(ex, "public async Task<bool> InsertAsync(TCollection entity)");
        }
        finally
        {
            await RemoveCache();
        }
        return false;
    }

    public async Task<bool> InsertManyAsync(List<TCollection> models)
    {
        try
        {
            foreach (var entity in models)
            {
                entity.CreatedDate = DateTime.UtcNow;
                entity.LastUpdatedDate = DateTime.UtcNow;
            }
            await _collectionContext.AddRangeAsync(models);
            var result = await _context.SaveChangesAsync();
            foreach (var entity in models)
            {
                await _distributedCache.SetAsync(Enum_RedisCacheKeys.ModelCaching, entity, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTime
                }, collection.CollectionName, entity.Id.ToString() ?? "ModelIdNotFound");
                await AddLog(entity, entity, Enum_OperationType.Insert);
            }
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            await RemoveCache();
        }
        return false;
    }

    public async Task<bool> UpdateAsync(TCollection entity)
    {
        try
        {
            entity.LastUpdatedDate = DateTime.UtcNow;
            TCollection? oldEntity = null;
            if (EnvironmentSettings.LoggingIsEnabled && entity.Id > 0)
            {
                oldEntity = await GetByIdAsync(entity.Id);
            }
            _context.Entry<TCollection>(entity).State = EntityState.Modified;
            _collectionContext.Update(entity);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                await AddLog(entity, oldEntity, Enum_OperationType.Update);
                await _distributedCache.SetAsync(Enum_RedisCacheKeys.ModelCaching, entity, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTime
                }, collection.CollectionName, entity.Id.ToString() ?? "ModelIdNotFound");
            }
            return result > 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            _logger.LogError(ex, "Task<bool> UpdateAsync(TCollection entity)");
        }
        finally
        {
            await RemoveCache();
        }
        return false;
    }
    public async Task<List<TCollection>?> GetAsync(LQ.Expression<Func<TCollection, bool>> predicate)
    {
        try
        {
            return await _collectionContext.Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: GetAsync " + predicate.ToString());
            return null;
        }
    }

    public async Task<List<TCollection>?> GetAsync<TKey>(
        Expression<Func<TCollection, bool>> predicate,
        Expression<Func<TCollection, TKey>> orderByPredicate,
        bool isDescending = false)
    {
        try
        {
            if (isDescending)
            {
                return await _collectionContext.OrderByDescending(orderByPredicate).Where(predicate).ToListAsync();
            }
            return await _collectionContext.OrderBy(orderByPredicate).Where(predicate).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: GetAsync ");
            return null;
        }
    }

    public async Task<List<TCollection>?> GetAsync<TKey>(
        Expression<Func<TCollection, bool>> predicate,
        Expression<Func<TCollection, TKey>> orderByPredicate,
        int limit,
        bool isDescending = false)
    {
        try
        {
            if (isDescending)
            {
                return await _collectionContext.OrderByDescending(orderByPredicate).Where(predicate).Take(limit).ToListAsync();
            }
            return await _collectionContext.OrderBy(orderByPredicate).Where(predicate).Take(limit).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: GetAsync ");
            return null;
        }
    }

    public async Task<List<TCollection>?> GetAsync<TKey>(
        Expression<Func<TCollection, bool>> predicate,
        Expression<Func<TCollection, TKey>> orderByPredicate,
        int page,
        int pageSize,
        bool isDescending = false)
    {
        try
        {
            int skip = (page - 1) * pageSize;
            int size = pageSize;
            if (skip < 0)
            {
                skip = 0;
            }
            if (size < 0)
            {
                size = 20;
            }
            if (isDescending)
            {
                return await _collectionContext.OrderByDescending(orderByPredicate)
                    .Where(predicate)
                    .Skip(skip).Take(pageSize)
                    .ToListAsync();
            }
            return await _collectionContext.OrderBy(orderByPredicate)
                .Where(predicate)
                .Skip(skip).Take(pageSize)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: GetAsync ");
            return null;
        }
    }

    public async Task<TCollection?> FirstAsync(LQ.Expression<Func<TCollection, bool>> predicate)
    {
        try
        {
            return await _collectionContext.FirstOrDefaultAsync(predicate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: GetAsync ");
            return null;
        }
    }

    public async Task<bool?> AnyAsync(LQ.Expression<Func<TCollection, bool>> predicate)
    {
        try
        {
            return await _collectionContext.AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: AnyAsync ");
            return null;
        }
    }

    public async Task<bool> DeleteAsync(LQ.Expression<Func<TCollection, bool>> predicate)
    {
        try
        {
            var getEntity = await GetAsync(predicate);
            if (getEntity?.Count > 0)
            {
                _collectionContext.RemoveRange(getEntity);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    foreach (var item in getEntity)
                    {
                        await AddLog(item, item, Enum_OperationType.Delete);
                        await _distributedCache.RemoveAsync(Enum_RedisCacheKeys.ModelCaching, collection.CollectionName, item.Id.ToString() ?? "ModelIdNotFound");
                    }
                }
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: DeleteAsync ");
        }
        finally
        {
            await RemoveCache();
        }
        return false;
    }

    public async Task<TCollection?> GetByIdAsync(int id)
    {
        var cacheResult = await _distributedCache.GetAsync<TCollection>(Enum_RedisCacheKeys.ModelCaching, collection.CollectionName, id.ToString());
        if (cacheResult != null && cacheResult.Id > 0)
        {
            return cacheResult;
        }
        try
        {
            var result = await _collectionContext.FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                await _distributedCache.SetAsync(Enum_RedisCacheKeys.ModelCaching, result, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTime
                }, collection.CollectionName, id.ToString());
            }
            return result;
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "Task<TCollection?> GetByIdAsync(string id)");
            return null;
        }
    }

    public async Task<List<TCollection>?> GetByIdsAsync(int[] ids)
    {
        var dataList = new List<TCollection>();
        foreach (var item in ids)
        {
            var cacheResult = await _distributedCache.GetAsync<TCollection>(Enum_RedisCacheKeys.ModelCaching, collection.CollectionName, item.ToString());
            if (cacheResult != null && cacheResult.Id > 0)
            {
                dataList.Add(cacheResult);
            }
        }
        if (dataList.Count == ids.Length)
        {
            return dataList;
        }
        var result = await _collectionContext.Where(x => ids.Contains(x.Id)).ToListAsync();
        if (result != null)
        {
            foreach (var item in result)
            {
                await _distributedCache.SetAsync(Enum_RedisCacheKeys.ModelCaching, item, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = _cacheTime
                }, collection.CollectionName, item.Id.ToString() ?? "ModelIdNotFound");
            }
        }
        return result;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        try
        {
            TCollection? removedEntity = await GetByIdAsync(id);
            if (removedEntity != null)
            {
                _collectionContext.Remove(removedEntity);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await AddLog(removedEntity, removedEntity, Enum_OperationType.Delete);
                    await _distributedCache.RemoveAsync(Enum_RedisCacheKeys.ModelCaching, collection.CollectionName, id.ToString());
                }
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MongoContext<TCollection>: DeleteAsync ");
        }
        finally
        {
            await RemoveCache();
        }
        return false;
    }
}