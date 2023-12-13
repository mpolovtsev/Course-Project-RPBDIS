using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HeatEnergyConsumption.Extensions;

namespace HeatEnergyConsumption.Models
{
    public partial class ProducedProduct
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public int OrganizationId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ВИД ПРОДУКЦИИ")]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "КОЛИЧЕСТВО ПРОИЗВЕДЁННОЙ ПРОДУКЦИИ")]
        public int ProductQuantity { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "КОЛИЧЕСТВО ЗАТРАЧЕННОЙ ТЕПЛОЭНЕРГИИ")]
        public float HeatEnergyQuantity { get; set; }

        [Display(Name = "ДАТА")]
        public DateTime Date { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "МЕСЯЦ")]
        public int Month { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ГОД")]
        public int Year { get; set; }

        [NotMapped]
        [Display(Name = "КВАРТАЛ")]
        public int DisplayedQuarter
        {
            get => Date.GetQuarter();
        }

        [NotMapped]
        [Display(Name = "ГОД")]
        public int DisplayedYear
        {
            get => Date.Year;
        }

        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public virtual Organization Organization { get; set; } = null!;

        [Display(Name = "ВИД ПРОДУКЦИИ")]
        public virtual ProductsType ProductType { get; set; } = null!;

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.ProducedProductsViewModel ProducedProductsViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}