using Microsoft.AspNetCore.Identity;

namespace HeatEnergyConsumption.ViewModels.UserViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<(IdentityUser User, string Role)> UsersRoles {  get; set; }
    }
}