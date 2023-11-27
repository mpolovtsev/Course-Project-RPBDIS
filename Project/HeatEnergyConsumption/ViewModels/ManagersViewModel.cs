using System.ComponentModel.DataAnnotations;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ManagersViewModel
    {
        public IEnumerable<Manager> Managers { get; set; }

        // Свойства для фильтрации
        [Display(Name = "Имя")]
        public string Name { get; set; }
        
        [Display(Name = "Фамилия")]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; }

        // Свойство для навигации по страницам
        public PageViewModel PageViewModel { get; set; }
    }
}
