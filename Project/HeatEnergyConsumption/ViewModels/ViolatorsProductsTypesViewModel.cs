using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ViolatorsProductsTypesViewModel
    {
        public IEnumerable<ViolatorProductsType> ViolatorsProductsTypes { get; set; }

        public ViolatorsProductsTypesFilterViewModel FilterViewModel { get; set; }

        public ViolatorsProductsTypesSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}