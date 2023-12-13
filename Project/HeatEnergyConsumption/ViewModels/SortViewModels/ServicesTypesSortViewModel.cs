using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ServicesTypesSortViewModel
    {
        public ServicesTypesSortViewModel() { }

        public ServicesTypesSortViewModel(ServicesTypesSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            CodeOrder = sortOrder == ServicesTypesSortState.CodeAsc ? ServicesTypesSortState.CodeDesc : ServicesTypesSortState.CodeAsc;
            NameOrder = sortOrder == ServicesTypesSortState.NameAsc ? ServicesTypesSortState.NameDesc : ServicesTypesSortState.NameAsc;
        }

        public ServicesTypesSortState CurrentOrder { get; set; }

        public ServicesTypesSortState CodeOrder { get; set; }

        public ServicesTypesSortState NameOrder { get; set; }
    }
}