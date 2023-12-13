using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class HeatEnergyConsumptionRatesViewModel
    {
        public IEnumerable<HeatEnergyConsumptionRate> HeatEnergyConsumptionRates { get; set; }

        public HeatEnergyConsumptionRatesFilterViewModel FilterViewModel { get; set; }

        public HeatEnergyConsumptionRatesSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}