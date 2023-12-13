using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.Extensions;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.SortStates;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.Controllers
{
    public class ChiefPowerEngineersController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ChiefPowerEngineersController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ChiefPowerEngineersFilterViewModel filterViewModel,
            ChiefPowerEngineersSortState sortOrder = ChiefPowerEngineersSortState.NameAsc, int page = 1)
        {
            IEnumerable<ChiefPowerEngineer> chiefPowerEngineers = dbContext.ChiefPowerEngineers
                                                                  .Include(chiefPowerEngineer => chiefPowerEngineer.Organization);

            if (chiefPowerEngineers == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ChiefPowerEngineerName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("ChiefPowerEngineerSurname", out string? surnameCookie);
                HttpContext.Request.Cookies.TryGetValue("ChiefPowerEngineerMiddleName", out string? middleNameCookie);
                HttpContext.Request.Cookies.TryGetValue("ChiefPowerEngineerOrganization", out string? organizationCookie);

                if (!(string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(surnameCookie) &&
                    string.IsNullOrEmpty(middleNameCookie) && string.IsNullOrEmpty(organizationCookie)))
                {
                    chiefPowerEngineers = chiefPowerEngineers.Filter(nameCookie, surnameCookie, middleNameCookie, 
                        organizationCookie);
                    filterViewModel.Name = nameCookie;
                    filterViewModel.Surname = surnameCookie;
                    filterViewModel.MiddleName = middleNameCookie;
                    filterViewModel.Organization = organizationCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Name) && string.IsNullOrEmpty(filterViewModel.Surname) &&
                    string.IsNullOrEmpty(filterViewModel.MiddleName) && string.IsNullOrEmpty(filterViewModel.Organization)))
                {
                    chiefPowerEngineers = chiefPowerEngineers.Filter(filterViewModel.Name, filterViewModel.Surname,
                        filterViewModel.MiddleName, filterViewModel.Organization);

                    if (!string.IsNullOrEmpty(filterViewModel.Name))
                        HttpContext.Response.Cookies.Append("ChiefPowerEngineerName", filterViewModel.Name);
                    else
                        HttpContext.Response.Cookies.Delete("ChiefPowerEngineerName");

                    if (!string.IsNullOrEmpty(filterViewModel.Surname))
                        HttpContext.Response.Cookies.Append("ChiefPowerEngineerSurname", filterViewModel.Surname);
                    else
                        HttpContext.Response.Cookies.Delete("ChiefPowerEngineerSurname");

                    if (!string.IsNullOrEmpty(filterViewModel.MiddleName))
                        HttpContext.Response.Cookies.Append("ChiefPowerEngineerMiddleName", filterViewModel.MiddleName);
                    else
                        HttpContext.Response.Cookies.Delete("ChiefPowerEngineerMiddleName");

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("ChiefPowerEngineerOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("ChiefPowerEngineerOrganization");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ChiefPowerEngineerName");
                    HttpContext.Response.Cookies.Delete("ChiefPowerEngineerSurname");
                    HttpContext.Response.Cookies.Delete("ChiefPowerEngineerMiddleName");
                    HttpContext.Response.Cookies.Delete("ChiefPowerEngineerOrganization");
                }
            }

            // Сортировка
            chiefPowerEngineers = chiefPowerEngineers.Sort(sortOrder);
            ChiefPowerEngineersSortViewModel sortViewModel = new ChiefPowerEngineersSortViewModel(sortOrder);

            // Разбиение на страницы
            int count = chiefPowerEngineers.Count();
            chiefPowerEngineers = chiefPowerEngineers.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            ChiefPowerEngineersViewModel model = new ChiefPowerEngineersViewModel()
            {
                ChiefPowerEngineers = chiefPowerEngineers,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.ChiefPowerEngineers == null)
                return NotFound();

            var chiefPowerEngineer = await dbContext.ChiefPowerEngineers
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chiefPowerEngineer == null)
                return NotFound();

            return View(chiefPowerEngineer);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,MiddleName,PhoneNumber,OrganizationId")] ChiefPowerEngineer chiefPowerEngineer)
        {
            if (ModelState.ErrorCount <= 1)
            {
                dbContext.Add(chiefPowerEngineer);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name");

            return View(chiefPowerEngineer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.ChiefPowerEngineers == null)
                return NotFound();

            var chiefPowerEngineer = await dbContext.ChiefPowerEngineers.FindAsync(id);

            if (chiefPowerEngineer == null)
                return NotFound();

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name");

            return View(chiefPowerEngineer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,MiddleName,PhoneNumber,OrganizationId")] ChiefPowerEngineer chiefPowerEngineer)
        {
            if (id != chiefPowerEngineer.Id)
                return NotFound();

            if (ModelState.ErrorCount <= 1)
            {
                try
                {
                    dbContext.Update(chiefPowerEngineer);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiefPowerEngineerExists(chiefPowerEngineer.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name");

            return View(chiefPowerEngineer);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.ChiefPowerEngineers == null)
                return NotFound();

            var chiefPowerEngineer = await dbContext.ChiefPowerEngineers
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (chiefPowerEngineer == null)
                return NotFound();

            return View(chiefPowerEngineer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.ChiefPowerEngineers == null)
                return NotFound();

            var chiefPowerEngineer = await dbContext.ChiefPowerEngineers.FindAsync(id);

            if (chiefPowerEngineer == null)
                return NotFound();

            dbContext.ChiefPowerEngineers.Remove(chiefPowerEngineer);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ChiefPowerEngineerExists(int id)
        {
            return (dbContext.ChiefPowerEngineers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}