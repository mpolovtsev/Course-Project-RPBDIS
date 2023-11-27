using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HeatEnergyConsumption.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppUsersController : Controller
    {
        UserManager<IdentityUser> userManager;
        RoleManager<IdentityRole> roleManager;

        public AppUsersController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            List<(IdentityUser, string)> usersRoles = new();
            IList<string> roles;

            foreach (IdentityUser user in userManager.Users)
            {
                roles = await userManager.GetRolesAsync(user);
                usersRoles.Add((user, string.Join<string>(" ", roles)));
            }

            UsersViewModel model = new UsersViewModel()
            {
                UsersRoles = usersRoles
            };

            return View(model);
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
                IdentityUser user = new IdentityUser 
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

            IdentityUser user = await userManager.FindByIdAsync(id);
            string role = (await userManager.GetRolesAsync(user))[0];

            if (user == null)
                return NotFound();

            EditUserViewModel model = new EditUserViewModel { Id = user.Id, Email = user.Email, Role = role };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByIdAsync(model.Id);

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
            IdentityUser user = await userManager.FindByIdAsync(id);
            string role = (await userManager.GetRolesAsync(user))[0];

            if (user == null)
                return NotFound();

            DeleteUserViewModel model = new DeleteUserViewModel { Id = user.Id, Email = user.Email, Role = role };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteUserViewModel model)
        {
            IdentityUser user = await userManager.FindByIdAsync(model.Id);

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