namespace HeatEnergyConsumption.Services.CacheService
{
    public interface ICacheService<T, K>
    {
        public void Add(K cacheKey);
        public IEnumerable<T> Get(K cacheKey);
    }
}