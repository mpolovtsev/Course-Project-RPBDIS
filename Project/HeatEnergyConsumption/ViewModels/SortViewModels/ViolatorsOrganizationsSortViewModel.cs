using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ViolatorsOrganizationsSortViewModel
    {
        public ViolatorsOrganizationsSortViewModel() { }

        public ViolatorsOrganizationsSortViewModel(ViolatorsOrganizationsSortState sortOrder) 
        {
            CurrentOrder = sortOrder;

            OrganizationOrder = sortOrder == ViolatorsOrganizationsSortState.OrganizationAsc ?
                ViolatorsOrganizationsSortState.OrganizationDesc : ViolatorsOrganizationsSortState.OrganizationAsc;
            ProductTypeOrder = sortOrder == ViolatorsOrganizationsSortState.ProductTypeAsc ?
                ViolatorsOrganizationsSortState.ProductTypeDesc : ViolatorsOrganizationsSortState.ProductTypeAsc;
            DifferenceOrder = sortOrder == ViolatorsOrganizationsSortState.DifferenceAsc ?
                ViolatorsOrganizationsSortState.DifferenceDesc : ViolatorsOrganizationsSortState.DifferenceAsc;
            YearOrder = sortOrder == ViolatorsOrganizationsSortState.YearAsc ?
                ViolatorsOrganizationsSortState.YearDesc : ViolatorsOrganizationsSortState.YearAsc;
        }

        public ViolatorsOrganizationsSortState CurrentOrder {  get; set; }

        public ViolatorsOrganizationsSortState OrganizationOrder { get; set; }

        public ViolatorsOrganizationsSortState ProductTypeOrder { get; set; }

        public ViolatorsOrganizationsSortState DifferenceOrder { get; set; }

        public ViolatorsOrganizationsSortState YearOrder { get; set; }
    }
}