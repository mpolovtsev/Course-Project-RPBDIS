using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.UserViewModels;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppUsersController : Controller
    {
        UserManager<AppUser> userManager;
        RoleManager<IdentityRole> roleManager;

        public AppUsersController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            List<AppUser> users = new List<AppUser>();
            IList<string> roles;

            foreach (AppUser user in userManager.Users)
            {
                roles = await userManager.GetRolesAsync(user);
                user.Roles = string.Join<string>(" ", roles);
                users.Add(user);
            }

            UsersViewModel model = new UsersViewModel()
            {
                Users = users
            };

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || userManager.Users == null)
                return NotFound();

            AppUser user = await userManager.FindByIdAsync(id);
            string role = (await userManager.GetRolesAsync(user))[0];

            if (user == null)
                return NotFound();

            user.Roles = role;

            return View(user);
        }

        public IActionResult Create()
        {
            ViewData["Roles"] = roleManager.Roles.Select(role => role.Name).Select(role => new SelectListItem()
            {
                Text = role,
                Value = role
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await userManager.CreateAsync(user, model.Password);

                await userManager.AddToRoleAsync(user, model.Role);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewData["Roles"] = roleManager.Roles.Select(role => role.Name).Select(role => new SelectListItem()
            {
                Text = role,
                Value = role
            });

            AppUser user = await userManager.FindByIdAsync(id);
            string role = (await userManager.GetRolesAsync(user))[0];

            if (user == null)
                return NotFound();

            EditUserViewModel model = new EditUserViewModel 
            { 
                Id = user.Id, 
                Email = user.Email, 
                Role = role 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    var result = await userManager.UpdateAsync(user);

                    var userRoles = await userManager.GetRolesAsync(user);
                    await userManager.RemoveFromRolesAsync(user, userRoles);
                    await userManager.AddToRoleAsync(user, model.Role);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            string role = (await userManager.GetRolesAsync(user))[0];

            if (user == null)
                return NotFound();

            DeleteUserViewModel model = new DeleteUserViewModel 
            { 
                Id = user.Id, 
                Email = user.Email, 
                Role = role 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteUserViewModel model)
        {
            AppUser user = await userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction(nameof(Index));
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
    }
}