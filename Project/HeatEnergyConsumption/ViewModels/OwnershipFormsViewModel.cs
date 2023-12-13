using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class OwnershipFormsViewModel
    {
        public IEnumerable<OwnershipForm> OwnershipForms { get; set; }

        public OwnershipFormsFilterViewModel FilterViewModel { get; set; }

        public OwnershipFormsSortViewModel SortViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}