using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using Microsoft.AspNetCore.Authorization;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeatEnergyConsumption.Controllers
{
    [Authorize]
    public class ManagersController : Controller
    {
        readonly HeatEnergyConsumptionContext _context;
        const int pageSize = 10;

        public ManagersController(HeatEnergyConsumptionContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string name, string surname, string middleName, SortState sortOrder = SortState.ManagerNameAsc, int page = 1)
        {
            // Загрузка данных из кэша
            IQueryable<Manager> managers = _context.Managers;

            if (managers == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (!(string.IsNullOrEmpty(name) && string.IsNullOrEmpty(surname) && string.IsNullOrEmpty(middleName)))
                managers = Filter(managers, name, surname, middleName);

            // Сортировка
            managers = Sort(managers, sortOrder);

            // Разбиение на страницы
            int count = await managers.CountAsync();
            managers = managers.Skip((page - 1) * pageSize).Take(pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            ManagersViewModel model = new ManagersViewModel()
            {
                Managers = managers,
                Name = name,
                Surname = surname,
                MiddleName = middleName,
                PageViewModel = pageViewModel
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,MiddleName,PhoneNumber")] Manager manager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(manager);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(manager);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers.FindAsync(id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,MiddleName,PhoneNumber")] Manager manager)
        {
            if (id != manager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(manager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ManagerExists(manager.Id))
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
            return View(manager);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Managers == null)
            {
                return NotFound();
            }

            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manager == null)
            {
                return NotFound();
            }

            return View(manager);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Managers == null)
            {
                return Problem("Записи не найдены.");
            }

            var manager = await _context.Managers.FindAsync(id);

            if (manager != null)
            {
                _context.Managers.Remove(manager);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ManagerExists(int id)
        {
            return (_context.Managers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        // middleName может быть null
        IQueryable<Manager> Filter(IQueryable<Manager> managers, string name, string surname, string middleName)
        {
            return managers.Where(manager => manager.Name.Contains(name ?? "") && manager.Surname.Contains(surname ?? "") && manager.MiddleName.Contains(middleName ?? ""));
        }

        IQueryable<Manager> Sort(IQueryable<Manager> managers, SortState sortOrder = SortState.ManagerNameAsc)
        {
            ViewData["NameSort"] = sortOrder == SortState.ManagerNameAsc ? SortState.ManagerNameDesc : SortState.ManagerNameAsc;
            ViewData["SurnameSort"] = sortOrder == SortState.ManagerSurnameAsc ? SortState.ManagerSurnameDesc : SortState.ManagerSurnameAsc;
            ViewData["MiddleNameSort"] = sortOrder == SortState.ManagerMiddleNameAsc ? SortState.ManagerMiddleNameDesc : SortState.ManagerMiddleNameAsc;

            return sortOrder switch
            {
                SortState.ManagerNameDesc => managers.OrderByDescending(manager => manager.Name),
                SortState.ManagerSurnameAsc => managers.OrderBy(manager => manager.Surname),
                SortState.ManagerSurnameDesc => managers.OrderByDescending(manager => manager.Surname),
                SortState.ManagerMiddleNameAsc => managers.OrderBy(manager => manager.MiddleName),
                SortState.ManagerMiddleNameDesc => managers.OrderByDescending(manager => manager.MiddleName),
                _ => managers.OrderBy(manager => manager.Name)
            };
        }
    }
}