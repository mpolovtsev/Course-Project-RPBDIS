namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ViolatorsOrganizationsFilterViewModel
    {
        public ViolatorsOrganizationsFilterViewModel() { }

        public ViolatorsOrganizationsFilterViewModel(string? organization, string? productType, double? difference, int? quarter, 
            int? year)
        {
            Organization = organization;
            ProductType = productType;
            Difference = difference;
            Quarter = quarter;
            Year = year;
        }

        public string? Organization { get; set; }

        public string? ProductType {  get; set; }

        public double? Difference { get; set; }

        public int? Quarter { get; set; }

        public int? Year { get; set; }
    }
}