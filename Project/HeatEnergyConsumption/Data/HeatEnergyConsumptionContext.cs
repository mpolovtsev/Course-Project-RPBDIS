using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Data
{
    public partial class HeatEnergyConsumptionContext : DbContext
    {
        public HeatEnergyConsumptionContext() { }

        public HeatEnergyConsumptionContext(DbContextOptions<HeatEnergyConsumptionContext> options)
            : base(options) { }

        public virtual DbSet<ChiefPowerEngineer> ChiefPowerEngineers { get; set; } = null!;
        public virtual DbSet<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; } = null!;
        public virtual DbSet<Manager> Managers { get; set; } = null!;
        public virtual DbSet<Organization> Organizations { get; set; } = null!;
        public virtual DbSet<OwnershipForm> OwnershipForms { get; set; } = null!;
        public virtual DbSet<ProducedProduct> ProducedProducts { get; set; } = null!;
        public virtual DbSet<ProductsType> ProductsTypes { get; set; } = null!;
        public virtual DbSet<ProvidedService> ProvidedServices { get; set; } = null!;
        public virtual DbSet<ServicesType> ServicesTypes { get; set; } = null!;
    }
}