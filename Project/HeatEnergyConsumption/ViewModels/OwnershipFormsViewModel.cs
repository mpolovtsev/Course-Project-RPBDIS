using System.ComponentModel.DataAnnotations;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class OwnershipFormsViewModel
    {
        public IEnumerable<OwnershipForm> OwnershipForms { get; set; }

        // Свойство для фильтрации
        [Display(Name = "Название")]
        public string Name { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
    }
}
