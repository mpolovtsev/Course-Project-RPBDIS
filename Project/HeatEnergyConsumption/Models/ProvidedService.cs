using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HeatEnergyConsumption.Extensions;

namespace HeatEnergyConsumption.Models
{
    public partial class ProvidedService
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public int OrganizationId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ВИД УСЛУГИ")]
        public int ServiceTypeId { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "КОЛИЧЕСТВО ОКАЗАННЫХ УСЛУГ")]
        public int Quantity { get; set; }

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

        [Display(Name = "ВИД УСЛУГИ")]
        public virtual ServicesType ServiceType { get; set; } = null!;

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.ProvidedServicesViewModel ProvidedServicesViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}