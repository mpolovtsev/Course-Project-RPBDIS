using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.ViewModels.UserViewModels
{
    public class DeleteUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "EMAIL")]
        public string Email { get; set; }

        [Display(Name = "РОЛЬ")]
        public string Role { get; set; }
    }
}
