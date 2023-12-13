using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class OrganizationsSortViewModel
    {   
        public OrganizationsSortViewModel() { }

        public OrganizationsSortViewModel(OrganizationsSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            NameOrder = sortOrder == OrganizationsSortState.NameAsc ? 
                OrganizationsSortState.NameDesc : OrganizationsSortState.NameAsc;
            OwnershipFormOrder = sortOrder == OrganizationsSortState.OwnershipFormAsc ? 
                OrganizationsSortState.OwnershipFormDesc: OrganizationsSortState.OwnershipFormAsc;
            ManagerOrder = sortOrder == OrganizationsSortState.ManagerAsc ? 
                OrganizationsSortState.ManagerDesc : OrganizationsSortState.ManagerAsc;
        }

        public OrganizationsSortState CurrentOrder { get; set; }

        public OrganizationsSortState NameOrder { get; set; }

        public OrganizationsSortState OwnershipFormOrder {  get; set; }

        public OrganizationsSortState ManagerOrder { get; set; }
    }
}