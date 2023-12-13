using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ServicesTypesViewModel
    {
        public IEnumerable<ServicesType> ServicesTypes { get; set; }

        public ServicesTypesFilterViewModel FilterViewModel { get; set; }

        public ServicesTypesSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}