using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public partial class ProductsType
    {
        public ProductsType()
        {
            HeatEnergyConsumptionRates = new HashSet<HeatEnergyConsumptionRate>();
            ProducedProducts = new HashSet<ProducedProduct>();
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

        public virtual ICollection<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; }

        public virtual ICollection<ProducedProduct> ProducedProducts { get; set; }

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.ProductsTypesViewModel ProductsTypesViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}