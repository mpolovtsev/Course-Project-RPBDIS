using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ChiefPowerEngineersViewModel
    {
        public IEnumerable<ChiefPowerEngineer> ChiefPowerEngineers { get; set; }

        public ChiefPowerEngineersFilterViewModel FilterViewModel { get; set; }

        public ChiefPowerEngineersSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}