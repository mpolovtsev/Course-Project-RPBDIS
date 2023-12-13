namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ProvidedServicesFilterViewModel
    {
        public ProvidedServicesFilterViewModel() { }

        public ProvidedServicesFilterViewModel(string? organization, string? serviceType, int? quantity, int? quarter, int? year)
        {
            Organization = organization;
            ServiceType = serviceType;
            Quantity = quantity;
            Quarter = quarter;
            Year = year;
        }

        public string? Organization { get; set; }

        public string? ServiceType {  get; set; }

        public int? Quantity { get; set; }

        public int? Quarter { get; set; }

        public int? Year { get; set; }
    }
}