using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.ViewModels
{
    public class OrganizationsViewModel
    {
        public IEnumerable<Organization> Organizations { get; set; }

        public OrganizationsSortViewModel SortViewModel { get; set; }

        public OrganizationsFilterViewModel FilterViewModel { get; set; }

        public PageViewModel PageViewModel { get; set; }
    }
}