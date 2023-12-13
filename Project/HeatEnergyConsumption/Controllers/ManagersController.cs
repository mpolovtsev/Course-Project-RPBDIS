using Microsoft.AspNetCore.Mvc;
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
    public class ManagersController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ManagersController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ManagersFilterViewModel filterViewModel,
            ManagersSortState sortOrder = ManagersSortState.NameAsc, int page = 1)
        {
            IEnumerable<Manager> managers = dbContext.Managers;

            if (managers == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ManagerName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("ManagerSurname", out string? surnameCookie);
                HttpContext.Request.Cookies.TryGetValue("ManagerMiddleName", out string? middleNameCookie);

                if (!(string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(surnameCookie) && string.IsNullOrEmpty(middleNameCookie)))
                {
                    managers = managers.Filter(nameCookie, surnameCookie, middleNameCookie);
                    filterViewModel.Name = nameCookie;
                    filterViewModel.Surname = surnameCookie;
                    filterViewModel.MiddleName = middleNameCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Name) && string.IsNullOrEmpty(filterViewModel.Surname) &&
                    string.IsNullOrEmpty(filterViewModel.MiddleName)))
                {
                    managers = managers.Filter(filterViewModel.Name, filterViewModel.Surname, filterViewModel.MiddleName);

                    if (!string.IsNullOrEmpty(filterViewModel.Name))
                        HttpContext.Response.Cookies.Append("ManagerName", filterViewModel.Name);
                    else
                        HttpContext.Response.Cookies.Delete("ManagerName");

                    if (!string.IsNullOrEmpty(filterViewModel.Surname))
                        HttpContext.Response.Cookies.Append("ManagerSurname", filterViewModel.Surname);
                    else
                        HttpContext.Response.Cookies.Delete("ManagerSurname");

                    if (!string.IsNullOrEmpty(filterViewModel.MiddleName))
                        HttpContext.Response.Cookies.Append("ManagerMiddleName", filterViewModel.MiddleName);
                    else
                        HttpContext.Response.Cookies.Delete("ManagerMiddleName");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ManagerName");
                    HttpContext.Response.Cookies.Delete("ManagerSurname");
                    HttpContext.Response.Cookies.Delete("ManagerMiddleName");
                }  
            }
            
            // Сортировка
            managers = managers.Sort(sortOrder);
            ManagersSortViewModel sortViewModel = new ManagersSortViewModel(sortOrder);

            // Пагинация
            int count = managers.Count();
            managers = managers.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            ManagersViewModel model = new ManagersViewModel()
            {
                Managers = managers,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
                return NotFound();

            return View(manager);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,MiddleName,PhoneNumber")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(manager);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers.FindAsync(id);

            if (manager == null)
                return NotFound();

            return View(manager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,MiddleName,PhoneNumber")] Manager manager)
        {
            if (id != manager.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(manager);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
                return NotFound();

            return View(manager);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Managers == null)
                return NotFound();

            var manager = await dbContext.Managers.FindAsync(id);

            if (manager == null)
                return NotFound();

            dbContext.Managers.Remove(manager);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ManagerExists(int id)
        {
            return (dbContext.Managers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}