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
    public class ProductsTypesController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ProductsTypesController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ProductsTypesFilterViewModel filterViewModel, 
            ProductsTypesSortState sortOrder = ProductsTypesSortState.CodeAsc, int page = 1)
        {
            IEnumerable<ProductsType> productsTypes = dbContext.ProductsTypes;

            if (productsTypes == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ProductsTypeCode", out string? codeCookie);
                HttpContext.Request.Cookies.TryGetValue("ProductsTypeName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("ProductsTypeUnit", out string? unitCookie);

                if (!(string.IsNullOrEmpty(codeCookie) && string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(unitCookie)))
                {
                    productsTypes = productsTypes.Filter(codeCookie, nameCookie, unitCookie);
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
                    productsTypes = productsTypes.Filter(filterViewModel.Code, filterViewModel.Name, filterViewModel.Unit);

                    if (!string.IsNullOrEmpty(filterViewModel.Code))
                        HttpContext.Response.Cookies.Append("ProductsTypeCode", filterViewModel.Code);
                    else
                        HttpContext.Response.Cookies.Delete("ProductsTypeCode");

                    if (!string.IsNullOrEmpty(filterViewModel.Name))
                        HttpContext.Response.Cookies.Append("ProductsTypeName", filterViewModel.Name);
                    else
                        HttpContext.Response.Cookies.Delete("ProductsTypeName");

                    if (!string.IsNullOrEmpty(filterViewModel.Unit))
                        HttpContext.Response.Cookies.Append("ProductsTypeUnit", filterViewModel.Unit);
                    else
                        HttpContext.Response.Cookies.Delete("ProductsTypeUnit");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ProductsTypeCode");
                    HttpContext.Response.Cookies.Delete("ProductsTypeName");
                    HttpContext.Response.Cookies.Delete("ProductsTypeUnit");
                }
            }

            // Сортировка
            productsTypes = productsTypes.Sort(sortOrder);
            ProductsTypesSortViewModel sortViewModel = new ProductsTypesSortViewModel(sortOrder);

            // Пагинация
            int count = productsTypes.Count();
            productsTypes = productsTypes.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ProductsTypesViewModel model = new ProductsTypesViewModel()
            {
                ProductsTypes = productsTypes,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.ProductsTypes == null)
                return NotFound();

            var productsType = await dbContext.ProductsTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (productsType == null)
                return NotFound();

            return View(productsType);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Code,Name,Unit")] ProductsType productsType)
        {
            if (ModelState.IsValid)
            {
                dbContext.Add(productsType);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(productsType);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.ProductsTypes == null)
                return NotFound();

            var productsType = await dbContext.ProductsTypes.FindAsync(id);

            if (productsType == null)
                return NotFound();

            return View(productsType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Name,Unit")] ProductsType productsType)
        {
            if (id != productsType.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    dbContext.Update(productsType);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsTypeExists(productsType.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            return View(productsType);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.ProductsTypes == null)
                return NotFound();

            var productsType = await dbContext.ProductsTypes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (productsType == null)
                return NotFound();

            return View(productsType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.ProductsTypes == null)
                return NotFound();

            var productsType = await dbContext.ProductsTypes.FindAsync(id);

            if (productsType == null)
                return NotFound();
            
            dbContext.ProductsTypes.Remove(productsType);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ProductsTypeExists(int id)
        {
          return (dbContext.ProductsTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}