using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ProvidedServicesViewModel
    {
        public IEnumerable<ProvidedService> ProvidedServices { get; set; }

        public ProvidedServicesFilterViewModel FilterViewModel { get; set; }

        public ProvidedServicesSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}