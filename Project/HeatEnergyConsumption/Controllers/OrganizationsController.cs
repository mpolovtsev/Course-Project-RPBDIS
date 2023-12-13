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
    public class OrganizationsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public OrganizationsController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(OrganizationsFilterViewModel filterViewModel, 
            OrganizationsSortState sortOrder = OrganizationsSortState.NameAsc, int page = 1)
        {
            IEnumerable<Organization> organizations = dbContext.Organizations
                .Include(organization => organization.OwnershipForm)
                .Include(organization => organization.Manager);

            if (organizations == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("OrganizationName", out string? nameCookie);
                HttpContext.Request.Cookies.TryGetValue("OrganizationOwnershipForm", out string? ownershipFormCookie);
                HttpContext.Request.Cookies.TryGetValue("OrganizationAddress", out string? addressCookie);
                HttpContext.Request.Cookies.TryGetValue("OrganizationManager", out string? managerCookie);

                if (!(string.IsNullOrEmpty(nameCookie) && string.IsNullOrEmpty(ownershipFormCookie) && string.IsNullOrEmpty(addressCookie) && 
                    string.IsNullOrEmpty(managerCookie)))
                {
                    organizations = organizations.Filter(nameCookie, ownershipFormCookie, addressCookie, managerCookie);
                    filterViewModel.Name = nameCookie;
                    filterViewModel.OwnershipForm = ownershipFormCookie;
                    filterViewModel.Address = addressCookie;
                    filterViewModel.Manager = managerCookie;
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Name) && string.IsNullOrEmpty(filterViewModel.OwnershipForm) &&
                    string.IsNullOrEmpty(filterViewModel.Address) && string.IsNullOrEmpty(filterViewModel.Manager)))
                {
                    organizations = organizations.Filter(filterViewModel.Name, filterViewModel.OwnershipForm, filterViewModel.Address, 
                        filterViewModel.Manager);

                    if (!string.IsNullOrEmpty(filterViewModel.Name))
                        HttpContext.Response.Cookies.Append("OrganizationName", filterViewModel.Name);
                    else
                        HttpContext.Response.Cookies.Delete("OrganizationName");

                    if (!string.IsNullOrEmpty(filterViewModel.OwnershipForm))
                        HttpContext.Response.Cookies.Append("OrganizationOwnershipForm", filterViewModel.OwnershipForm);
                    else
                        HttpContext.Response.Cookies.Delete("OrganizationOwnershipForm");

                    if (!string.IsNullOrEmpty(filterViewModel.Address))
                        HttpContext.Response.Cookies.Append("OrganizationAddress", filterViewModel.Address);
                    else
                        HttpContext.Response.Cookies.Delete("OrganizationAddress");

                    if (!string.IsNullOrEmpty(filterViewModel.Manager))
                        HttpContext.Response.Cookies.Append("OrganizationManager", filterViewModel.Manager);
                    else
                        HttpContext.Response.Cookies.Delete("OrganizationManager");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("OrganizationName");
                    HttpContext.Response.Cookies.Delete("OrganizationOwnershipForm");
                    HttpContext.Response.Cookies.Delete("OrganizationAddress");
                    HttpContext.Response.Cookies.Delete("OrganizationManager");
                }
            }

            // Сортировка
            organizations = organizations.Sort(sortOrder);
            OrganizationsSortViewModel sortViewModel = new OrganizationsSortViewModel(sortOrder);

            // Пагинация
            int count = organizations.Count();
            organizations = organizations.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            OrganizationsViewModel model = new OrganizationsViewModel()
            {
                Organizations = organizations,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations
                .Include(o => o.Manager)
                .Include(o => o.OwnershipForm)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
                return NotFound();

            return View(organization);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Surname");
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,OwnershipFormId,Address,ManagerId")] Organization organization)
        {
            if (ModelState.ErrorCount <= 2)
            {
                dbContext.Add(organization);
                await dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Surname", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Name", organization.OwnershipFormId);

            return View(organization);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations.FindAsync(id);

            if (organization == null)
                return NotFound();

            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Surname", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Id", "Name", organization.OwnershipFormId);

            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OwnershipFormId,Address,ManagerId")] Organization organization)
        {
            if (id != organization.Id)
                return NotFound();

            if (ModelState.ErrorCount <= 2)
            {
                try
                {
                    dbContext.Update(organization);
                    await dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ManagerId"] = new SelectList(dbContext.Managers, "Id", "Surname", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(dbContext.OwnershipForms, "Name", "Id", organization.OwnershipFormId);

            return View(organization);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations
                .Include(o => o.Manager)
                .Include(o => o.OwnershipForm)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
                return NotFound();

            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (dbContext.Organizations == null)
                return NotFound();

            var organization = await dbContext.Organizations.FindAsync(id);

            if (organization == null)
                return NotFound();

            dbContext.Organizations.Remove(organization);
            await dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool OrganizationExists(int id)
        {
            return (dbContext.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}