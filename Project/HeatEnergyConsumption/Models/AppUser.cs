using Microsoft.AspNetCore.Identity;

namespace HeatEnergyConsumption.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public string Roles { get; set; }

        public Data.ApplicationDbContext ApplicationDbContext
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.UserViewModels.CreateUserViewModel CreateUserViewModel
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.UserViewModels.EditUserViewModel EditUserViewModel
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.UserViewModels.DeleteUserViewModel DeleteUserViewModel
        {
            get => default;
            set
            {
            }
        }

        public ViewModels.UserViewModels.UsersViewModel UsersViewModel
        {
            get => default;
            set
            {
            }
        }
    }
}