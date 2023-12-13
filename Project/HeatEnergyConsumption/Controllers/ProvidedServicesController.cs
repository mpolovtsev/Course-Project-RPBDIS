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
    public class ProvidedServicesController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ProvidedServicesController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ProvidedServicesFilterViewModel filterViewModel, 
            ProvidedServicesSortState sortOrder = ProvidedServicesSortState.OrganizationAsc, int page = 1)
        {
            IEnumerable<ProvidedService> providedServices = dbContext.ProvidedServices
                .Include(providedService => providedService.Organization)
                .Include(providedService => providedService.ServiceType);

            if (providedServices == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ProvidedServiceOrganization", out string? organizationCookie);
                HttpContext.Request.Cookies.TryGetValue("ProvidedServiceServiceType", out string? serviceTypeCookie);
                HttpContext.Request.Cookies.TryGetValue("ProvidedServiceQuantity", out string? quantityCookie);
                HttpContext.Request.Cookies.TryGetValue("ProvidedServiceQuarter", out string? quarterCookie);
                HttpContext.Request.Cookies.TryGetValue("ProvidedServiceYear", out string? yearCookie);

                if (!(string.IsNullOrEmpty(organizationCookie) && string.IsNullOrEmpty(serviceTypeCookie) && string.IsNullOrEmpty(quantityCookie) &&
                    string.IsNullOrEmpty(quarterCookie) && string.IsNullOrEmpty(yearCookie)))
                {
                    filterViewModel.Organization = organizationCookie;
                    filterViewModel.ServiceType = serviceTypeCookie;
                    filterViewModel.Quantity = int.TryParse(quantityCookie, out int quantity) ? quantity : null;
                    filterViewModel.Quarter = int.TryParse(quarterCookie, out int quarter) ? quarter : null;
                    filterViewModel.Year = int.TryParse(yearCookie, out int year) ? year : null;
                    providedServices = providedServices.Filter(filterViewModel.Organization, filterViewModel.ServiceType, filterViewModel.Quantity,
                        filterViewModel.Quarter, filterViewModel.Year);
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Organization) && string.IsNullOrEmpty(filterViewModel.ServiceType) && filterViewModel.Quantity != null &&
                    filterViewModel.Quarter != null && filterViewModel.Year != null))
                {
                    providedServices = providedServices.Filter(filterViewModel.Organization, filterViewModel.ServiceType, filterViewModel.Quantity,
                        filterViewModel.Quarter, filterViewModel.Year);

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("ProvidedServiceOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("ProvidedServiceOrganization");

                    if (!string.IsNullOrEmpty(filterViewModel.ServiceType))
                        HttpContext.Response.Cookies.Append("ProvidedServiceServiceType", filterViewModel.ServiceType);
                    else
                        HttpContext.Response.Cookies.Delete("ProvidedServiceServiceType");

                    if (filterViewModel.Quantity != null)
                        HttpContext.Response.Cookies.Append("ProvidedServiceQuantity", filterViewModel.Quantity.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProvidedServiceQuantity");

                    if (filterViewModel.Quarter != null)
                        HttpContext.Response.Cookies.Append("ProvidedServiceQuarter", filterViewModel.Quarter.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProvidedServiceQuarter");

                    if (filterViewModel.Year != null)
                        HttpContext.Response.Cookies.Append("ProvidedServiceYear", filterViewModel.Year.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ProvidedServiceYear");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ProvidedServiceOrganization");
                    HttpContext.Response.Cookies.Delete("ProvidedServiceServiceType");
                    HttpContext.Response.Cookies.Delete("ProvidedServiceQuantity");
                    HttpContext.Response.Cookies.Delete("ProvidedServiceQuarter");
                    HttpContext.Response.Cookies.Delete("ProvidedServiceYear");
                }
            }

            // Сортировка
            providedServices = providedServices.Sort(sortOrder);
            ProvidedServicesSortViewModel sortViewModel = new ProvidedServicesSortViewModel(sortOrder);

            // Пагинация
            int count = providedServices.Count();
            providedServices = providedServices.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ProvidedServicesViewModel model = new ProvidedServicesViewModel()
            {
                ProvidedServices = providedServices,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.ProvidedServices == null)
                return NotFound();

            var providedService = await dbContext.ProvidedServices
                .Include(s => s.Organization)
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (providedService == null)
                return NotFound();

            return View(providedService);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name");
            ViewData["ServiceTypeId"] = new SelectList(dbContext.ServicesTypes, "Id", "Name");
            ViewData["Months"] = GetMonths();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,OrganizationId,ServiceTypeId,Quantity,Month,Year")] ProvidedService providedService)
        {
            if (ModelState.ErrorCount <= 3)
            {
                providedService.Date = new DateTime(providedService.Year, providedService.Month, 1);
                dbContext.Add(providedService);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", providedService.OrganizationId);
            ViewData["ServiceTypeId"] = new SelectList(dbContext.ServicesTypes, "Id", "Name", providedService.ServiceTypeId);
            ViewData["Months"] = GetMonths();

            return View(providedService);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.ProvidedServices == null)
                return NotFound();

            var providedService = await dbContext.ProvidedServices.FindAsync(id);

            if (providedService == null)
                return NotFound();

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Name", providedService.OrganizationId);
            ViewData["ServiceTypeId"] = new SelectList(dbContext.ServicesTypes, "Id", "Name", providedService.ServiceTypeId);
            ViewData["Months"] = GetMonths();

            return View(providedService);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrganizationId,ServiceTypeId,Quantity,Date")] ProvidedService providedService)
        {
            if (id != providedService.Id)
                return NotFound();

            if (ModelState.ErrorCount <= 3)
            {
                try
                {
                    providedService.Date = new DateTime(providedService.Year, providedService.Month, 1);
                    dbContext.Update(providedService);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvidedServiceExists(providedService.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["OrganizationId"] = new SelectList(dbContext.Organizations, "Id", "Id", providedService.OrganizationId);
            ViewData["ServiceTypeId"] = new SelectList(dbContext.ServicesTypes, "Id", "Id", providedService.ServiceTypeId);
            ViewData["Months"] = GetMonths();

            return View(providedService);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.ProvidedServices == null)
                return NotFound();

            var providedService = await dbContext.ProvidedServices
                .Include(s => s.Organization)
                .Include(s => s.ServiceType)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (providedService == null)
                return NotFound();

            return View(providedService);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.ProvidedServices == null)
                return NotFound();

            var providedService = await dbContext.ProvidedServices.FindAsync(id);

            if (providedService == null)
                return NotFound();

            dbContext.ProvidedServices.Remove(providedService);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ProvidedServiceExists(int id)
        {
            return (dbContext.ProvidedServices?.Any(e => e.Id == id)).GetValueOrDefault();
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