using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "НАЗВАНИЕ")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ФОРМА СОБСТВЕННОСТИ")]
        public int OwnershipFormId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "АДРЕС")]
        public string Address { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "РУКОВОДИТЕЛЬ")]
        public int? ManagerId { get; set; }

        [Display(Name = "ФОРМА СОБСТВЕННОСТИ")]
        public virtual OwnershipForm OwnershipForm { get; set; } = null!;

        [Display(Name = "РУКОВОДИТЕЛЬ")]
        public virtual Manager? Manager { get; set; }

        public virtual ICollection<ChiefPowerEngineer> ChiefPowerEngineers { get; set; }

        public virtual ICollection<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; }

        public virtual ICollection<ProducedProduct> ProducedProducts { get; set; }

        public virtual ICollection<ProvidedService> ProvidedServices { get; set; }

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.OrganizationsViewModel OrganizationsViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}