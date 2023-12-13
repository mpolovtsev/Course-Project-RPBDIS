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
    public class ServicesTypesController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ServicesTypesController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ServicesTypesFilterViewModel filterViewModel, 
            ServicesTypesSortState sortOrder = ServicesTypesSortState.CodeAsc, int page = 1)
        {
            IEnumerable<ServicesType> servicesTypes = dbContext.ServicesTypes;

            if (servicesTypes == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ServicesTypeCode", out string? codeCookie);
                HttpContext.Request.Cookies.TryGetValue("ServicesTypeName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("ServicesTypeUnit", out string? unitCookie);

                if (!(string.IsNullOrEmpty(codeCookie) && string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(unitCookie)))
                {
                    servicesTypes = servicesTypes.Filter(codeCookie, nameCookie, unitCookie);
                    filterViewModel.Code = codeCookie;
                    filterViewModel.Name = nameCookie;
                    filterViewModel.Unit = unitCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Code) && string.IsNullOrEmpty(filterViewModel.Name) && 
                    string.IsNullOrEmpty(filterViewModel.Unit)))
                {
                    servicesTypes = servicesTypes.Filter(filterViewModel.Code, filterViewModel.Name, filterViewModel.Unit);

                    if (!string.IsNullOrEmpty(filterViewModel.Code))
                        HttpContext.Response.Cookies.Append("ServicesTypeCode", filterViewModel.Code);
                    else
                        HttpContext.Response.Cookies.Delete("ServicesTypeCode");

                    if (!string.IsNullOrEmpty(filterViewModel.Name))
                        HttpContext.Response.Cookies.Append("ServicesTypeName", filterViewModel.Name);
                    else
                        HttpContext.Response.Cookies.Delete("ServicesTypeName");

                    if (!string.IsNullOrEmpty(filterViewModel.Unit))
                        HttpContext.Response.Cookies.Append("ServicesTypeUnit", filterViewModel.Unit);
                    else
                        HttpContext.Response.Cookies.Delete("ServicesTypeUnit");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ServicesTypeCode");
                    HttpContext.Response.Cookies.Delete("ServicesTypeName");
                    HttpContext.Response.Cookies.Delete("ServicesTypeUnit");
                }
            }

            // Сортировка
            servicesTypes = servicesTypes.Sort(sortOrder);
            ServicesTypesSortViewModel sortViewModel = new ServicesTypesSortViewModel(sortOrder);

            // Пагинация
            int count = servicesTypes.Count();
            servicesTypes = servicesTypes.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ServicesTypesViewModel model = new ServicesTypesViewModel()
            {
                ServicesTypes = servicesTypes,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.ServicesTypes == null)
                return NotFound();

            var servicesType = await dbContext.ServicesTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servicesType == null)
                return NotFound();

            return View(servicesType);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Unit")] ServicesType servicesType)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(servicesType);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(servicesType);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.ServicesTypes == null)
                return NotFound();

            var servicesType = await dbContext.ServicesTypes.FindAsync(id);

            if (servicesType == null)
                return NotFound();

            return View(servicesType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Unit")] ServicesType servicesType)
        {
            if (id != servicesType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(servicesType);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicesTypeExists(servicesType.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(servicesType);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.ServicesTypes == null)
                return NotFound();

            var servicesType = await dbContext.ServicesTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servicesType == null)
                return NotFound();

            return View(servicesType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.ServicesTypes == null)
                return NotFound();

            var servicesType = await dbContext.ServicesTypes.FindAsync(id);

            if (servicesType == null)
                return NotFound();
            
            dbContext.ServicesTypes.Remove(servicesType);
            await dbContext.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        bool ServicesTypeExists(int id)
        {
            return (dbContext.ServicesTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}