using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HeatEnergyConsumption.Models
{
    public partial class Organization
    {
        public Organization()
        {
            ChiefPowerEngineers = new HashSet<ChiefPowerEngineer>();
            HeatEnergyConsumptionRates = new HashSet<HeatEnergyConsumptionRate>();
            ProducedProducts = new HashSet<ProducedProduct>();
            ProvidedServices = new HashSet<ProvidedService>();
        }

        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;
        [Display(Name = "Форма собственности")]
        [ForeignKey("OwnershipForm")]
        public int OwnershipFormId { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; } = null!;
        [Display(Name = "Руководитель")]
        [ForeignKey("Manager")]
        public int? ManagerId { get; set; }

        public virtual Manager? Manager { get; set; }
        public virtual OwnershipForm OwnershipForm { get; set; } = null!;
        public virtual ICollection<ChiefPowerEngineer> ChiefPowerEngineers { get; set; }
        public virtual ICollection<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; }
        public virtual ICollection<ProducedProduct> ProducedProducts { get; set; }
        public virtual ICollection<ProvidedService> ProvidedServices { get; set; }
    }
}