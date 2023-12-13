using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public partial class Manager
    {
        public Manager()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name="ИМЯ")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "ФАМИЛИЯ")]
        public string Surname { get; set; } = null!;

        [Display(Name = "ОТЧЕСТВО")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "НОМЕР ТЕЛЕФОНА")]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Organization> Organizations { get; set; }

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.ManagersViewModel ManagersViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}