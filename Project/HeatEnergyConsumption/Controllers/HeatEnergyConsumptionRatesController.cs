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
    public class HeatEnergyConsumptionRatesController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public HeatEnergyConsumptionRatesController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(HeatEnergyConsumptionRatesFilterViewModel filterViewModel, 
            HeatEnergyConsumptionRatesSortState sortOrder = HeatEnergyConsumptionRatesSortState.OrganizationAsc, int page = 1)
        {
             IEnumerable<HeatEnergyConsumptionRate> heatEnergyConsumptionRates = dbContext.HeatEnergyConsumptionRates
                .Include(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Organization)
                .Include(heatEnergyConsumptionRate => heatEnergyConsumptionRate.ProductType);

            if (heatEnergyConsumptionRates == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("HeatEnergyConsumptionRateOrganization", out string? organizationCookie);
                HttpContext.Request.Cookies.TryGetValue("HeatEnergyConsumptionRateProductType", out string? productTypeCookie);
                HttpContext.Request.Cookies.TryGetValue("HeatEnergyConsumptionRateQuantity", out string? quantityCookie);
                HttpContext.Request.Cookies.TryGetValue("HeatEnergyConsumptionRateQuarter", out string? quarterCookie);
                HttpContext.Request.Cookies.TryGetValue("HeatEnergyConsumptionRateYear", out string? yearCookie);

                if (!(string.IsNullOrEmpty(organizationCookie) && string.IsNullOrEmpty(productTypeCookie) && string.IsNullOrEmpty(quantityCookie) &&
                    string.IsNullOrEmpty(quarterCookie) && string.IsNullOrEmpty(yearCookie)))
                {
                    filterViewModel.Organization = organizationCookie;
                    filterViewModel.ProductType = productTypeCookie;
                    filterViewModel.Quantity = int.TryParse(quantityCookie, out int quantity) ? quantity : null;
                    filterViewModel.Quarter = int.TryParse(quarterCookie, out int quarter) ? quarter : null;
                    filterViewModel.Year = int.TryParse(yearCookie, out int year) ? year : null;
                    heatEnergyConsumptionRates = heatEnergyConsumptionRates.Filter(filterViewModel.Organization, filterViewModel.ProductType, 
                        filterViewModel.Quantity, filterViewModel.Quarter, filterViewModel.Year);
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Organization) && string.IsNullOrEmpty(filterViewModel.ProductType) && 
                    filterViewModel.Quantity == null && filterViewModel.Quarter == null && filterViewModel.Year == null))
                {
                    heatEnergyConsumptionRates = heatEnergyConsumptionRates.Filter(filterViewModel.Organization, filterViewModel.ProductType, 
                        filterViewModel.Quantity, filterViewModel.Quarter, filterViewModel.Year);

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("HeatEnergyConsumptionRateOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateOrganization");

                    if (!string.IsNullOrEmpty(filterViewModel.ProductType))
                        HttpContext.Response.Cookies.Append("HeatEnergyConsumptionRateProductType", filterViewModel.ProductType);
                    else
                        HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateProductType");

                    if (filterViewModel.Quantity != null)
                        HttpContext.Response.Cookies.Append("HeatEnergyConsumptionRateQuantity", filterViewModel.Quantity.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateQuantity");

                    if (filterViewModel.Quarter != null)
                        HttpContext.Response.Cookies.Append("HeatEnergyConsumptionRateQuarter", filterViewModel.Quarter.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateQuarter");

                    if (filterViewModel.Year != null)
                        HttpContext.Response.Cookies.Append("HeatEnergyConsumptionRateYear", filterViewModel.Year.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateYear");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateOrganization");
                    HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateProductType");
                    HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateQuantity");
                    HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateQuarter");
                    HttpContext.Response.Cookies.Delete("HeatEnergyConsumptionRateYear");
                }
            }

            // Сортировка
            heatEnergyConsumptionRates = heatEnergyConsumptionRates.Sort(sortOrder);
            HeatEnergyConsumptionRatesSortViewModel sortViewModel = new HeatEnergyConsumptionRatesSortViewModel(sortOrder);

            // Пагинация
            int count = heatEnergyConsumptionRates.Count();
            heatEnergyConsumptionRates = heatEnergyConsumptionRates.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            HeatEnergyConsumptionRatesViewModel model = new HeatEnergyConsumptionRatesViewModel()
            {
                HeatEnergyConsumptionRates = heatEnergyConsumptionRates,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.HeatEnergyConsumptionRates == null)
                return NotFound();

            var heatEnergyConsumptionRate = await dbContext.HeatEnergyConsumptionRates
                .Include(r => r.Organization)
                .Include(r => r.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (heatEnergyConsumptionRate == null)
                return NotFound();

            return View(heatEnergyConsumptionRate);
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
        public async Task<IActionResult> Create([Bind("Id,OrganizationId,ProductTypeId,Quantity,Month,Year")] HeatEnergyConsumptionRate heatEnergyConsumptionRate)
        {
            if (ModelState.ErrorCount <= 3)
            {
                heatEnergyConsumptionRate.Date = new DateTime(heatEnergyConsumptionRate.Year, heatEnergyConsumptionRate.Month, 1);
                dbContext.Add(heatEnergyConsumptionRate);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", heatEnergyConsumptionRate.OrganizationId);
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name", heatEnergyConsumptionRate.ProductTypeId);
            ViewData["Months"] = GetMonths();

            return View(heatEnergyConsumptionRate);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.HeatEnergyConsumptionRates == null)
                return NotFound();

            var heatEnergyConsumptionRate = await dbContext.HeatEnergyConsumptionRates.FindAsync(id);

            if (heatEnergyConsumptionRate == null)
                return NotFound();

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", heatEnergyConsumptionRate.OrganizationId);
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name", heatEnergyConsumptionRate.ProductTypeId);
            ViewData["Months"] = GetMonths();

            return View(heatEnergyConsumptionRate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganizationId,ProductTypeId,Quantity,Month,Year")] HeatEnergyConsumptionRate heatEnergyConsumptionRate)
        {
            if (id != heatEnergyConsumptionRate.Id)
                return NotFound();

            if (ModelState.ErrorCount <= 3)
            {
                try
                {
                    heatEnergyConsumptionRate.Date = new DateTime(heatEnergyConsumptionRate.Year, heatEnergyConsumptionRate.Month, 1);
                    dbContext.Update(heatEnergyConsumptionRate);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeatEnergyConsumptionRateExists(heatEnergyConsumptionRate.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", heatEnergyConsumptionRate.OrganizationId);
            ViewData["ProductTypeId"] = new SelectList(dbContext.ProductsTypes, "Id", "Name", heatEnergyConsumptionRate.ProductTypeId);
            ViewData["Months"] = GetMonths();

            return View(heatEnergyConsumptionRate);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.HeatEnergyConsumptionRates == null)
                return NotFound();

            var heatEnergyConsumptionRate = await dbContext.HeatEnergyConsumptionRates
                .Include(r => r.Organization)
                .Include(r => r.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (heatEnergyConsumptionRate == null)
                return NotFound();

            return View(heatEnergyConsumptionRate);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.HeatEnergyConsumptionRates == null)
                return NotFound();

            var heatEnergyConsumptionRate = await dbContext.HeatEnergyConsumptionRates.FindAsync(id);

            if (heatEnergyConsumptionRate == null)
                return NotFound();
            
            dbContext.HeatEnergyConsumptionRates.Remove(heatEnergyConsumptionRate);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool HeatEnergyConsumptionRateExists(int id)
        {
          return (dbContext.HeatEnergyConsumptionRates?.Any(e => e.Id == id)).GetValueOrDefault();
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