using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ManagersViewModel
    {
        public IEnumerable<Manager> Managers { get; set; }

        public ManagersFilterViewModel FilterViewModel { get; set; }

        public ManagersSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}