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
        [Display(Name="Имя")]
        public string Name { get; set; } = null!;
        [Display(Name = "Фамилия")]
        public string Surname { get; set; } = null!;
        [Display(Name = "Отчество")]
        public string? MiddleName { get; set; }
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; } = null!;

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}