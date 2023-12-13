using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class OwnershipFormsSortViewModel
    {
        public OwnershipFormsSortViewModel() { }

        public OwnershipFormsSortViewModel(OwnershipFormsSortState sortOrder) 
        {
            CurrentOrder = sortOrder;

            NameOrder = sortOrder == OwnershipFormsSortState.NameAsc ? 
                OwnershipFormsSortState.NameDesc: OwnershipFormsSortState.NameAsc;
        }

        public OwnershipFormsSortState CurrentOrder { get; set; }

        public OwnershipFormsSortState NameOrder { get; set; }
    }
}