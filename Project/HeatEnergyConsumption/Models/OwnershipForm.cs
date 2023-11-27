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
        [Display(Name = "Название")]
        public string Name { get; set; } = null!;

        public virtual ICollection<Organization> Organizations { get; set; }
    }
}