using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            if (!roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
                await roleManager.CreateAsync(new IdentityRole(model.Name));

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IdentityRole role)
        {
            IdentityRole updatedRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);

            if (updatedRole == null)
                return NotFound();

            updatedRole.Name = role.Name;
            await roleManager.UpdateAsync(updatedRole);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(IdentityRole role)
        {
            IdentityRole deletedRole = await roleManager.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);

            if (deletedRole == null)
                return NotFound();

            await roleManager.DeleteAsync(role);

            return RedirectToAction(nameof(Index));
        }
    }
}