namespace HeatEnergyConsumption.Extensions
{
    public static class PaginationExtension
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> items, int page = 1, int pageSize = 10)
        {
            return items.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}