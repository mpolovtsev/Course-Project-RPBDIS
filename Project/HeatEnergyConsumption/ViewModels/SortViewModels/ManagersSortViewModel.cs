using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ManagersSortViewModel
    {
        public ManagersSortViewModel(ManagersSortState sortOrder) 
        {
            CurrentOrder = sortOrder;

            NameOrder = sortOrder == ManagersSortState.NameAsc ? 
                ManagersSortState.NameDesc : ManagersSortState.NameAsc;
            SurnameOrder = sortOrder == ManagersSortState.SurnameAsc ? 
                ManagersSortState.SurnameDesc : ManagersSortState.SurnameAsc;
            MiddleNameOrder = sortOrder == ManagersSortState.MiddleNameAsc ? 
                ManagersSortState.MiddleNameDesc : ManagersSortState.MiddleNameAsc;
        }

        public ManagersSortState CurrentOrder { get; set; }

        public ManagersSortState NameOrder { get; set; }
        
        public ManagersSortState SurnameOrder { get; set; }

        public ManagersSortState MiddleNameOrder { get; set; } 
    }
}