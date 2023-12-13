using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class HeatEnergyConsumptionRatesSortViewModel
    {
        public HeatEnergyConsumptionRatesSortViewModel() { } 

        public HeatEnergyConsumptionRatesSortViewModel(HeatEnergyConsumptionRatesSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            OrganizationOrder = sortOrder == HeatEnergyConsumptionRatesSortState.OrganizationAsc ?
                HeatEnergyConsumptionRatesSortState.OrganizationDesc : HeatEnergyConsumptionRatesSortState.OrganizationAsc;
            ProductTypeOrder = sortOrder == HeatEnergyConsumptionRatesSortState.ProductTypeAsc ?
                HeatEnergyConsumptionRatesSortState.ProductTypeDesc : HeatEnergyConsumptionRatesSortState.ProductTypeAsc;
            QuantityOrder = sortOrder == HeatEnergyConsumptionRatesSortState.QuantityAsc ?
                HeatEnergyConsumptionRatesSortState.QuantityDesc : HeatEnergyConsumptionRatesSortState.QuantityAsc;
            YearOrder = sortOrder == HeatEnergyConsumptionRatesSortState.YearAsc ?
                HeatEnergyConsumptionRatesSortState.YearDesc : HeatEnergyConsumptionRatesSortState.YearAsc;
        }

        public HeatEnergyConsumptionRatesSortState CurrentOrder { get; set; }

        public HeatEnergyConsumptionRatesSortState OrganizationOrder { get; set; }

        public HeatEnergyConsumptionRatesSortState ProductTypeOrder { get; set; }

        public HeatEnergyConsumptionRatesSortState QuantityOrder { get; set; }

        public HeatEnergyConsumptionRatesSortState YearOrder { get; set; }
    }
}