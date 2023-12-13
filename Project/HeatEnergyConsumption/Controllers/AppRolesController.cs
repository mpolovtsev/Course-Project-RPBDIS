using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.ViewModels;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AppRolesController : Controller
    {
        readonly RoleManager<IdentityRole> roleManager;

        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            IEnumerable<IdentityRole> roles = roleManager.Roles;

            RolesViewModel model = new RolesViewModel()
            {
                Roles = roles
            };

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null || roleManager.Roles == null)
                return NotFound();

            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
                return NotFound();

            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!await roleManager.RoleExistsAsync(model.Name))
                await roleManager.CreateAsync(new IdentityRole(model.Name));

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole updatedRole = await roleManager.FindByIdAsync(id);

            if (updatedRole == null)
                return NotFound();

            return View(updatedRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            IdentityRole? updatedRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);

            if (updatedRole == null)
                return NotFound();

            updatedRole.Name = role.Name;
            await roleManager.UpdateAsync(updatedRole);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole deletedRole = await roleManager.FindByIdAsync(id);

            if (deletedRole == null)
                return NotFound();

            return View(deletedRole);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IdentityRole role)
        {
            IdentityRole? deletedRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);

            if (deletedRole == null)
                return NotFound();

            await roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
    }
}