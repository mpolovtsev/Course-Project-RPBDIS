using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.Extensions;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortStates;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.Controllers
{
    public class OwnershipFormsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public OwnershipFormsController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(OwnershipFormsFilterViewModel filterViewModel, 
            OwnershipFormsSortState sortOrder = OwnershipFormsSortState.NameAsc, int page = 1)
        {
            IEnumerable<OwnershipForm> ownershipForms = dbContext.OwnershipForms;

            if (ownershipForms == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("OwnershipFormName", out string? nameCookie);

                if (!string.IsNullOrEmpty(nameCookie))
                {
                    ownershipForms = ownershipForms.Filter(nameCookie);
                    filterViewModel.Name = nameCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!string.IsNullOrEmpty(filterViewModel.Name))
                {
                    ownershipForms = ownershipForms.Filter(filterViewModel.Name);
                    HttpContext.Response.Cookies.Append("OwnershipFormName", filterViewModel.Name);
                }
                else
                    HttpContext.Response.Cookies.Delete("OwnershipFormName");
            }
 
            // Сортировка
            ownershipForms = ownershipForms.Sort(sortOrder);
            OwnershipFormsSortViewModel sortViewModel = new OwnershipFormsSortViewModel(sortOrder);

            // Пагинация
            int count = ownershipForms.Count();
            ownershipForms = ownershipForms.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            OwnershipFormsViewModel model = new OwnershipFormsViewModel()
            {
                OwnershipForms = ownershipForms,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ownershipForm == null)
                return NotFound();

            return View(ownershipForm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] OwnershipForm ownershipForm)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(ownershipForm);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ownershipForm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms.FindAsync(id);

            if (ownershipForm == null)
                return NotFound();

            return View(ownershipForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] OwnershipForm ownershipForm)
        {
            if (id != ownershipForm.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(ownershipForm);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnershipFormExists(ownershipForm.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(ownershipForm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ownershipForm == null)
                return NotFound();

            return View(ownershipForm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.OwnershipForms == null)
                return NotFound();

            var ownershipForm = await dbContext.OwnershipForms.FindAsync(id);

            if (ownershipForm == null)
                return NotFound();

            dbContext.OwnershipForms.Remove(ownershipForm);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool OwnershipFormExists(int id)
        {
            return (dbContext.OwnershipForms?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}