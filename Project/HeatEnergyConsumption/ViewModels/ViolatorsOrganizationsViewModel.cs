using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class ViolatorsOrganizationsViewModel
    {
        public IEnumerable<ViolatorOrganization> ViolatorsOrganizations { get; set; }

        public ViolatorsOrganizationsFilterViewModel FilterViewModel { get; set; }

        public ViolatorsOrganizationsSortViewModel SortViewModel {  get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}