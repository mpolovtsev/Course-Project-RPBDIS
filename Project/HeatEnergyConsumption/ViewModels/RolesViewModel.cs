using Microsoft.AspNetCore.Identity;

namespace HeatEnergyConsumption.ViewModels
{
    public class RolesViewModel
    {
        public IEnumerable<IdentityRole> Roles { get; set; }
    }
}