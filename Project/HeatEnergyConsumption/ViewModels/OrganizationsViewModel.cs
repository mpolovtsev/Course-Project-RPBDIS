using System.ComponentModel.DataAnnotations;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class OrganizationsViewModel
    {
        public IEnumerable<Organization> Organizations { get; set; }

        // Свойства для фильтрации
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Руководитель")]
        public string Manager { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Display(Name = "Форма собственности")]
        public string OwnershipForm { get; set; }
        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
    }
}
