using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class OwnershipFormsCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<OwnershipForm, string>
    {
        public OwnershipFormsCacheService(HeatEnergyConsumptionContext dbContext, 
            IMemoryCache cache, int storageTime = 600) : base(dbContext, cache, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<OwnershipForm> ownershipForms = dbContext.OwnershipForms.Take(5);

            if (ownershipForms != null)
                cache.Set(cacheKey, ownershipForms, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<OwnershipForm> Get(string cacheKey)
        {
            IEnumerable<OwnershipForm> ownershipForms;

            if (!cache.TryGetValue(cacheKey, out ownershipForms))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<OwnershipForm>>(cacheKey);
            }

            return ownershipForms;
        }
    }
}