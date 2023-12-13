using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.ViewModels.UserViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}