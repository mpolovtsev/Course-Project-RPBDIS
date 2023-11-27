using Microsoft.Extensions.Caching.Memory;

namespace HeatEnergyConsumption.Services.CacheService
{
    public abstract class DataCacheServiceFromDB<T>
    {
        protected readonly T dbContext;
        protected readonly IMemoryCache cache;
        protected int storageTime;

        protected DataCacheServiceFromDB(T dbContext, IMemoryCache cache, 
            int storageTime)
        {
            this.dbContext = dbContext;
            this.cache = cache;
            this.storageTime = storageTime;
        }
    }
}