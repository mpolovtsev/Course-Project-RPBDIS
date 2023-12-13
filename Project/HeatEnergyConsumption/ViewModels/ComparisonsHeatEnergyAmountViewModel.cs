using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ComparisonsHeatEnergyAmountViewModel
    {
        public IEnumerable<ComparisonHeatEnergyAmount> ComparisonsHeatEnergyAmount { get; set; }

        public ComparisonsHeatEnergyAmountFilterViewModel FilterViewModel { get; set; }

        public ComparisonsHeatEnergyAmountSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}