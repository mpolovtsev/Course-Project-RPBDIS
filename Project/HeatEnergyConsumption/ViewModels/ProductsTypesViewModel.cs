using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ProductsTypesViewModel
    {
        public IEnumerable<ProductsType> ProductsTypes {  get; set; }

        public ProductsTypesFilterViewModel FilterViewModel {  get; set; }

        public ProductsTypesSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}