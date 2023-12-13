using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public partial class OwnershipForm
    {
        public OwnershipForm()
        {
            Organizations = new HashSet<Organization>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Это поле обязательно для заполнения.")]
        [Display(Name = "НАЗВАНИЕ")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Organization> Organizations { get; set; }

        public Data.HeatEnergyConsumptionContext HeatEnergyConsumptionContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.OwnershipFormsViewModel OwnershipFormsViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}