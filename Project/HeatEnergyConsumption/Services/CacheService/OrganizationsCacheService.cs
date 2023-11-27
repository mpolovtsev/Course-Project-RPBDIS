using Microsoft.Extensions.Caching.Memory;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Services.CacheService
{
    public class OrganizationsCacheService : 
        DataCacheServiceFromDB<HeatEnergyConsumptionContext>, 
        ICacheService<Organization, string>
    {
        public OrganizationsCacheService(HeatEnergyConsumptionContext dbContext,
            IMemoryCache cache, int storageTime = 2 * 24 + 240) : 
            base(dbContext, cache, storageTime) { }

        public void Add(string cacheKey)
        {
            IEnumerable<Organization> organizations = 
                dbContext.Organizations.ToList();

            if (organizations != null)
                cache.Set(cacheKey, organizations, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(storageTime)
                });
        }

        public IEnumerable<Organization> Get(string cacheKey)
        {
            IEnumerable<Organization> organizations;

            if (!cache.TryGetValue(cacheKey, out organizations))
            {
                Add(cacheKey);

                return cache.Get<IEnumerable<Organization>>(cacheKey);
            }

            return organizations;
        }
    }
}