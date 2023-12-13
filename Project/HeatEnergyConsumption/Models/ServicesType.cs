using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public partial class ServicesType
    {
        public ServicesType()
        {
            ProvidedServices = new HashSet<ProvidedService>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "КОД")]
        public string Code { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "НАЗВАНИЕ")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ЕДИНИЦА ИЗМЕРЕНИЯ")]
        public string Unit { get; set; } = null!;

        public virtual ICollection<ProvidedService> ProvidedServices { get; set; }

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.ServicesTypesViewModel ServicesTypesViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}