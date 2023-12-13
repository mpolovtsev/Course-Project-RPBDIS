using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ChiefPowerEngineersSortViewModel
    {
        public ChiefPowerEngineersSortViewModel(ChiefPowerEngineersSortState sortOrder) 
        {
            CurrentOrder = sortOrder;

            NameOrder = sortOrder == ChiefPowerEngineersSortState.NameAsc ? 
                ChiefPowerEngineersSortState.NameDesc : ChiefPowerEngineersSortState.NameAsc;
            SurnameOrder = sortOrder == ChiefPowerEngineersSortState.SurnameAsc ? 
                ChiefPowerEngineersSortState.SurnameDesc : ChiefPowerEngineersSortState.SurnameAsc;
            MiddleNameOrder = sortOrder == ChiefPowerEngineersSortState.MiddleNameAsc ? 
                ChiefPowerEngineersSortState.MiddleNameDesc : ChiefPowerEngineersSortState.MiddleNameAsc;
            OrganizationOrder = sortOrder == ChiefPowerEngineersSortState.OrganizationAsc ?
                ChiefPowerEngineersSortState.OrganizationDesc : ChiefPowerEngineersSortState.OrganizationAsc;
        }

        public ChiefPowerEngineersSortState CurrentOrder { get; set; }

        public ChiefPowerEngineersSortState NameOrder {  get; set; }

        public ChiefPowerEngineersSortState SurnameOrder {  get; set; }

        public ChiefPowerEngineersSortState MiddleNameOrder { get; set; }

        public ChiefPowerEngineersSortState OrganizationOrder { get; set; }
    }
}