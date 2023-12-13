using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ProducedProductsSortViewModel
    {
        public ProducedProductsSortViewModel() { }

        public ProducedProductsSortViewModel(ProducedProductsSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            OrganizationOrder = sortOrder == ProducedProductsSortState.OrganizationAsc ? 
                ProducedProductsSortState.OrganizationDesc : ProducedProductsSortState.OrganizationAsc;
            ProductTypeOrder = sortOrder == ProducedProductsSortState.ProductTypeAsc ? 
                ProducedProductsSortState.ProductTypeDesc : ProducedProductsSortState.ProductTypeAsc;
            ProductQuantityOrder = sortOrder == ProducedProductsSortState.ProductQuantityAsc ? 
                ProducedProductsSortState.ProductQuantityDesc : ProducedProductsSortState.ProductQuantityAsc;
            HeatEnergyQuantityOrder = sortOrder == ProducedProductsSortState.HeatEnergyQuantityAsc ? 
                ProducedProductsSortState.HeatEnergyQuantityDesc : ProducedProductsSortState.HeatEnergyQuantityAsc;
            YearOrder = sortOrder == ProducedProductsSortState.YearAsc ? 
                ProducedProductsSortState.YearDesc : ProducedProductsSortState.YearAsc;
        }

        public ProducedProductsSortState CurrentOrder {  get; set; }

        public ProducedProductsSortState OrganizationOrder {  get; set; }

        public ProducedProductsSortState ProductTypeOrder { get; set; }

        public ProducedProductsSortState ProductQuantityOrder { get; set; }

        public ProducedProductsSortState HeatEnergyQuantityOrder { get; set; }

        public ProducedProductsSortState YearOrder { get; set; }
    }
}