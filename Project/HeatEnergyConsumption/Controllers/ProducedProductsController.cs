using System.Globalization;
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
    public class ProducedProductsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ProducedProductsController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ProducedProductsFilterViewModel filterViewModel, 
            ProducedProductsSortState sortOrder = ProducedProductsSortState.OrganizationAsc, int page = 1)
        {
            IEnumerable<ProducedProduct> producedProducts = dbContext.ProducedProducts
                .Include(producedProduct => producedProduct.Organization)
                .Include(producedProduct => producedProduct.ProductType);

            if (producedProducts == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ProducedProductOrganization", out string? organizationCookie);
                HttpContext.Request.Cookies.TryGetValue("ProducedProductProductType", out string? productTypeCookie);
                HttpContext.Request.Cookies.TryGetValue("ProducedProductProductQuantity", out string? productQuantityCookie);
                HttpContext.Request.Cookies.TryGetValue("ProducedProductHeatEnergyQuantity", out string? heatEnergyQuantityCookie);
                HttpContext.Request.Cookies.TryGetValue("ProducedProductQuarter", out string? quarterCookie);
                HttpContext.Request.Cookies.TryGetValue("ProducedProductYear", out string? yearCookie);

                if (!(string.IsNullOrEmpty(organizationCookie) && string.IsNullOrEmpty(productTypeCookie) && string.IsNullOrEmpty(productQuantityCookie) &&
                    string.IsNullOrEmpty(heatEnergyQuantityCookie) && string.IsNullOrEmpty(quarterCookie) && string.IsNullOrEmpty(yearCookie)))
                {
                    filterViewModel.Organization = organizationCookie;
                    filterViewModel.ProductType = productTypeCookie;
                    filterViewModel.ProductQuantity = int.TryParse(productQuantityCookie, out int productQuantity) ? productQuantity : null;
                    filterViewModel.HeatEnergyQuantity = int.TryParse(heatEnergyQuantityCookie, out int heatEnergyQuantity) ? heatEnergyQuantity : null;
                    filterViewModel.Quarter = int.TryParse(quarterCookie, out int quarter) ? quarter : null;
                    filterViewModel.Year = int.TryParse(yearCookie, out int year) ? year : null;
                    producedProducts = producedProducts.Filter(filterViewModel.Organization, filterViewModel.ProductType, filterViewModel.ProductQuantity, 
                        filterViewModel.HeatEnergyQuantity, filterViewModel.Quarter, filterViewModel.Year);
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Organization) && string.IsNullOrEmpty(filterViewModel.ProductType) && filterViewModel.ProductQuantity != null && 
                    filterViewModel.HeatEnergyQuantity != null && filterViewModel.Quarter != null && filterViewModel.Year != null))
                {
                    producedProducts = producedProducts.Filter(filterViewModel.Organization, filterViewModel.ProductType, filterViewModel.ProductQuantity, 
                        filterViewModel.HeatEnergyQuantity, filterViewModel.Quarter, filterViewModel.Year);

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("ProducedProductOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("ProducedProductOrganization");

                    if (!string.IsNullOrEmpty(filterViewModel.ProductType))
                        HttpContext.Response.Cookies.Append("ProducedProductProductType", filterViewModel.ProductType);
                    else
                        HttpContext.Response.Cookies.Delete("ProducedProductProductType");

                    if (filterViewModel.ProductQuantity != null)
                        HttpContext.Response.Cookies.Append("ProducedProductProductQuantity", filterViewModel.ProductQuantity.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProducedProductProductQuantity");

                    if (filterViewModel.HeatEnergyQuantity != null)
                        HttpContext.Response.Cookies.Append("ProducedProductHeatEnergyQuantity", filterViewModel.HeatEnergyQuantity.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProducedProductHeatEnergyQuantity");

                    if (filterViewModel.Quarter != null)
                        HttpContext.Response.Cookies.Append("ProducedProductQuarter", filterViewModel.Quarter.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProducedProductQuarter");

                    if (filterViewModel.Year != null)
                        HttpContext.Response.Cookies.Append("ProducedProductYear", filterViewModel.Year.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProducedProductYear");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ProducedProductOrganization");
                    HttpContext.Response.Cookies.Delete("ProducedProductProductType");
                    HttpContext.Response.Cookies.Delete("ProducedProductProductQuantity");
                    HttpContext.Response.Cookies.Delete("ProducedProductHeatEnergyQuantity");
                    HttpContext.Response.Cookies.Delete("ProducedProductQuarter");
                    HttpContext.Response.Cookies.Delete("ProducedProductYear");
                }
            }

            // Сортировка
            producedProducts = producedProducts.Sort(sortOrder);
            ProducedProductsSortViewModel sortViewModel = new ProducedProductsSortViewModel(sortOrder);

            // Пагинация
            int count = producedProducts.Count();
            producedProducts = producedProducts.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ProducedProductsViewModel model = new ProducedProductsViewModel()
            {
                ProducedProducts = producedProducts,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.ProducedProducts == null)
                return NotFound();

            var producedProduct = await dbContext.ProducedProducts
                .Include(p => p.Organization)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producedProduct == null)
                return NotFound();

            return View(producedProduct);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name");
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name");
            ViewData["Months"] = GetMonths();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,OrganizationId,ProductTypeId,ProductQuantity,HeatEnergyQuantity,Month,Year")] ProducedProduct producedProduct)
        {
            if (ModelState.ErrorCount <= 3)
            {
                producedProduct.Date = new DateTime(producedProduct.Year, producedProduct.Month, 1);
                dbContext.Add(producedProduct);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", producedProduct.OrganizationId);
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name", producedProduct.ProductTypeId);
            ViewData["Months"] = GetMonths();

            return View(producedProduct);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.ProducedProducts == null)
                return NotFound();

            var producedProduct = await dbContext.ProducedProducts.FindAsync(id);

            if (producedProduct == null)
                return NotFound();

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", producedProduct.OrganizationId);
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name", producedProduct.ProductTypeId);
            ViewData["Months"] = GetMonths();

            return View(producedProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganizationId,ProductTypeId,ProductQuantity,HeatEnergyQuantity,Month,Year")] ProducedProduct producedProduct)
        {
            if (id != producedProduct.Id)
                return NotFound();

            if (ModelState.ErrorCount <= 3)
            {
                try
                {
                    producedProduct.Date = new DateTime(producedProduct.Year, producedProduct.Month, 1);
                    dbContext.Update(producedProduct);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProducedProductExists(producedProduct.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", producedProduct.OrganizationId);
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name", producedProduct.ProductTypeId);
            ViewData["Months"] = GetMonths();

            return View(producedProduct);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.ProducedProducts == null)
                return NotFound();

            var producedProduct = await dbContext.ProducedProducts
                .Include(p => p.Organization)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producedProduct == null)
                return NotFound();

            return View(producedProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.ProducedProducts == null)
                return NotFound();

            var producedProduct = await dbContext.ProducedProducts.FindAsync(id);

            if (producedProduct == null)
                return NotFound();

            dbContext.ProducedProducts.Remove(producedProduct);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ProducedProductExists(int id)
        {
          return (dbContext.ProducedProducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        List<SelectListItem> GetMonths()
        {
            List<SelectListItem> months = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
                months.Add(new SelectListItem { Text = monthName, Value = i.ToString() });
            }

            return months;
        }
    }
}