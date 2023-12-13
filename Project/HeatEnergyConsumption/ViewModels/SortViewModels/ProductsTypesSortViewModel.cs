using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ProductsTypesSortViewModel
    {
        public ProductsTypesSortViewModel() { }

        public ProductsTypesSortViewModel(ProductsTypesSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            CodeOrder = sortOrder == ProductsTypesSortState.CodeAsc ? ProductsTypesSortState.CodeDesc : ProductsTypesSortState.CodeAsc;
            NameOrder = sortOrder == ProductsTypesSortState.NameAsc ? ProductsTypesSortState.NameDesc : ProductsTypesSortState.NameAsc;
        }

        public ProductsTypesSortState CurrentOrder { get; set; }

        public ProductsTypesSortState CodeOrder { get; set; }

        public ProductsTypesSortState NameOrder { get; set; }
    }
}