using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ProducedProductsViewModel
    {
        public IEnumerable<ProducedProduct> ProducedProducts { get; set; }

        public ProducedProductsFilterViewModel FilterViewModel { get; set; }

        public ProducedProductsSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel {  get; set; }
    }
}