using HeatEnergyConsumption.Data;

namespace HeatEnergyConsumption.Middleware
{
    public class DbInitializerMiddleware
    {
        readonly RequestDelegate next;

        public DbInitializerMiddleware(RequestDelegate next) => this.next = next;

        public Task Invoke(HttpContext context, HeatEnergyConsumptionContext dbContext)
        {
            if (!context.Session.Keys.Contains("Starting"))
            {
                DbInitializer.InitializeDb(dbContext);
                context.Session.SetString("Starting", "Yes");
            }

            return next.Invoke(context);
        }
    }

    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder) =>
            builder.UseMiddleware<DbInitializerMiddleware>();
    }
}