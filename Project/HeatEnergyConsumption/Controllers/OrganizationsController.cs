using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HeatEnergyConsumption.ViewModels;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize]
    public class OrganizationsController : Controller
    {
        readonly HeatEnergyConsumptionContext _context;
        const int pageSize = 10;

        public OrganizationsController(HeatEnergyConsumptionContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(SortState sortOrder = SortState.OrganizationNameAsc, int page = 1)
        {
            // Загрузка данных из кэша
            IQueryable<Organization> organizations = _context.Organizations.Include(organization => organization.Manager).Include(organization => organization.OwnershipForm);
            
            if (organizations == null)
                return Problem("Записи не найдены.");

            // Фильтрация


            // Сортировка
            organizations = Sort(organizations, sortOrder);

            // Разбиение на страницы
            int count = await organizations.CountAsync();
            organizations = organizations.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            OrganizationsViewModel model = new OrganizationsViewModel()
            {
                Organizations = organizations,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .Include(o => o.Manager)
                .Include(o => o.OwnershipForm)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Surname");
            ViewData["OwnershipFormId"] = new SelectList(_context.OwnershipForms, "Id", "Name");
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,OwnershipFormId,Address,ManagerId")] Organization organization)
        {
            if (ModelState.ErrorCount <= 2)
            {
                _context.Add(organization);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Surname", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(_context.OwnershipForms, "Id", "Name", organization.OwnershipFormId);
            
            return View(organization);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
            {
                return NotFound();
            }

            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Id", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(_context.OwnershipForms, "Id", "Id", organization.OwnershipFormId);
            
            return View(organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,OwnershipFormId,Address,ManagerId")] Organization organization)
        {
            if (id != organization.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationExists(organization.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Id", organization.ManagerId);
            ViewData["OwnershipFormId"] = new SelectList(_context.OwnershipForms, "Id", "Id", organization.OwnershipFormId);
            
            return View(organization);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Organizations == null)
            {
                return NotFound();
            }

            var organization = await _context.Organizations
                .Include(o => o.Manager)
                .Include(o => o.OwnershipForm)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Organizations == null)
            {
                return Problem("Записи не найдены.");
            }

            var organization = await _context.Organizations.FindAsync(id);

            if (organization != null)
            {
                _context.Organizations.Remove(organization);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationExists(int id)
        {
            return (_context.Organizations?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        IQueryable<Organization> Sort(IQueryable<Organization> organizations, SortState sortOrder = SortState.OrganizationNameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.OrganizationNameAsc ? SortState.OrganizationNameDesc : SortState.OrganizationNameAsc;
            ViewData["ManagerSort"] = sortOrder == SortState.OrganizationManagerAsc ? SortState.OrganizationManagerDesc : SortState.OrganizationManagerAsc;

            return sortOrder switch
            {
                SortState.OrganizationNameDesc => organizations.OrderByDescending(organization => organization.Name),
                SortState.OrganizationManagerAsc => organizations.OrderBy(organization => organization.Manager.Surname),
                SortState.OrganizationManagerDesc => organizations.OrderByDescending(organization => organization.Manager.Surname),
                _ => organizations.OrderBy(organization => organization.Name)
            };
        }
    }
}