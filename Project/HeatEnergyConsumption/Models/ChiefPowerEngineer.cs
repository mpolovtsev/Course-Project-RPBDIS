using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public partial class ChiefPowerEngineer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ИМЯ")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ФАМИЛИЯ")]
        public string Surname { get; set; } = null!;

        [Display(Name = "ОТЧЕСТВО")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "НОМЕР ТЕЛЕФОНА")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public int OrganizationId { get; set; }

        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public virtual Organization Organization { get; set; } = null!;

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.ChiefPowerEngineersViewModel ChiefPowerEngineersViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}