using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.Services.CacheService;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize]
    public class OwnershipFormsController : Controller
    {
        readonly HeatEnergyConsumptionContext _context;
        const int pageSize = 10;

        public OwnershipFormsController(HeatEnergyConsumptionContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string name, SortState sortOrder = SortState.OwnershipFormNameAsc, int page = 1)
        {
            // Загрузка данных из кэша
            IQueryable<OwnershipForm> ownershipForms = _context.OwnershipForms;

            if (ownershipForms == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (!string.IsNullOrEmpty(name))
                ownershipForms = Filter(ownershipForms, name);

            // Сортировка
            ownershipForms = Sort(ownershipForms, sortOrder);

            // Разбиение на страницы
            int count = await ownershipForms.CountAsync();
            ownershipForms = ownershipForms.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            OwnershipFormsViewModel model = new OwnershipFormsViewModel()
            {
                OwnershipForms = ownershipForms,
                Name = name,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OwnershipForms == null)
            {
                return NotFound();
            }

            var ownershipForm = await _context.OwnershipForms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ownershipForm == null)
            {
                return NotFound();
            }

            return View(ownershipForm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] OwnershipForm ownershipForm)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ownershipForm);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ownershipForm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OwnershipForms == null)
            {
                return NotFound();
            }

            var ownershipForm = await _context.OwnershipForms.FindAsync(id);

            if (ownershipForm == null)
            {
                return NotFound();
            }

            return View(ownershipForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] OwnershipForm ownershipForm)
        {
            if (id != ownershipForm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ownershipForm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnershipFormExists(ownershipForm.Id))
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

            return View(ownershipForm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OwnershipForms == null)
            {
                return NotFound();
            }

            var ownershipForm = await _context.OwnershipForms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ownershipForm == null)
            {
                return NotFound();
            }

            return View(ownershipForm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OwnershipForms == null)
            {
                return Problem("Entity set 'HeatEnergyConsumptionContext.OwnershipForms'  is null.");
            }

            var ownershipForm = await _context.OwnershipForms.FindAsync(id);
            
            if (ownershipForm != null)
            {
                _context.OwnershipForms.Remove(ownershipForm);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool OwnershipFormExists(int id)
        {
            return (_context.OwnershipForms?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        IQueryable<OwnershipForm> Filter(IQueryable<OwnershipForm> ownershipForms, string name)
        {
            return ownershipForms.Where(ownershipForm => ownershipForm.Name.Contains(name ?? ""));
        }

        IQueryable<OwnershipForm> Sort(IQueryable<OwnershipForm> ownershipForms, SortState sortOrder = SortState.OwnershipFormNameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.OwnershipFormNameAsc ? SortState.OwnershipFormNameDesc : SortState.OwnershipFormNameAsc;

            return sortOrder switch
            {
                SortState.OwnershipFormNameDesc => ownershipForms.OrderByDescending(ownershipForm => ownershipForm.Name),
                _ => ownershipForms.OrderBy(ownershipForm => ownershipForm.Name)
            };
        }
    }
}
