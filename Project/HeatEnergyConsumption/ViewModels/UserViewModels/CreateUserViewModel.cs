using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace HeatEnergyConsumption.ViewModels.UserViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name ="Пароль")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}
