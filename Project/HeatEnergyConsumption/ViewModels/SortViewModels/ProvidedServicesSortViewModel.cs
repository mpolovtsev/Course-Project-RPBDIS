using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ProvidedServicesSortViewModel
    {
        public ProvidedServicesSortViewModel() { }

        public ProvidedServicesSortViewModel(ProvidedServicesSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            OrganizationOrder = sortOrder == ProvidedServicesSortState.OrganizationAsc ? 
                ProvidedServicesSortState.OrganizationDesc : ProvidedServicesSortState.OrganizationAsc;
            ServiceTypeOrder = sortOrder == ProvidedServicesSortState.ServiceTypeAsc ? 
                ProvidedServicesSortState.ServiceTypeDesc : ProvidedServicesSortState.ServiceTypeAsc;
            QuantityOrder = sortOrder == ProvidedServicesSortState.QuantityAsc ? 
                ProvidedServicesSortState.QuantityDesc : ProvidedServicesSortState.QuantityAsc;
            YearOrder = sortOrder == ProvidedServicesSortState.YearAsc ? 
                ProvidedServicesSortState.YearDesc : ProvidedServicesSortState.YearAsc;
        }

        public ProvidedServicesSortState CurrentOrder {  get; set; }

        public ProvidedServicesSortState OrganizationOrder { get; set; }

        public ProvidedServicesSortState ServiceTypeOrder { get; set; }

        public ProvidedServicesSortState QuantityOrder { get; set; }

        public ProvidedServicesSortState YearOrder { get; set; }
    }
}