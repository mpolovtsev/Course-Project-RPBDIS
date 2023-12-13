using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ComparisonsHeatEnergyAmountSortViewModel
    {
        public ComparisonsHeatEnergyAmountSortViewModel() { }

        public ComparisonsHeatEnergyAmountSortViewModel(ComparisonsHeatEnergyAmountSortState sortOrder) 
        {
            CurrentOrder = sortOrder;

            OrganizationOrder = sortOrder == ComparisonsHeatEnergyAmountSortState.OrganizationAsc ?
                ComparisonsHeatEnergyAmountSortState.OrganizationDesc : ComparisonsHeatEnergyAmountSortState.OrganizationAsc;
            ProductTypeOrder = sortOrder == ComparisonsHeatEnergyAmountSortState.ProductTypeAsc ?
                ComparisonsHeatEnergyAmountSortState.ProductTypeDesc : ComparisonsHeatEnergyAmountSortState.ProductTypeAsc;
            ActualHeatEnergyConsumptionOrder = sortOrder == ComparisonsHeatEnergyAmountSortState.ActualHeatEnergyConsumptionAsc ?
                ComparisonsHeatEnergyAmountSortState.ActualHeatEnergyConsumptionDesc : ComparisonsHeatEnergyAmountSortState.ActualHeatEnergyConsumptionAsc;
            NormalizedHeatEnergyConsumptionOrder = sortOrder == ComparisonsHeatEnergyAmountSortState.NormalizedHeatEnergyConsumptionAsc ?
                ComparisonsHeatEnergyAmountSortState.NormalizedHeatEnergyConsumptionDesc : ComparisonsHeatEnergyAmountSortState.NormalizedHeatEnergyConsumptionAsc;
            YearOrder = sortOrder == ComparisonsHeatEnergyAmountSortState.YearAsc ?
                ComparisonsHeatEnergyAmountSortState.YearDesc : ComparisonsHeatEnergyAmountSortState.YearAsc;
        }

        public ComparisonsHeatEnergyAmountSortState CurrentOrder { get; set; }

        public ComparisonsHeatEnergyAmountSortState OrganizationOrder { get; set; }

        public ComparisonsHeatEnergyAmountSortState ProductTypeOrder { get; set; }

        public ComparisonsHeatEnergyAmountSortState ActualHeatEnergyConsumptionOrder { get; set; }

        public ComparisonsHeatEnergyAmountSortState NormalizedHeatEnergyConsumptionOrder { get; set; }

        public ComparisonsHeatEnergyAmountSortState YearOrder { get; set; }
    }
}