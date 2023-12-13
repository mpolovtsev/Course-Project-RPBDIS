namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class HeatEnergyConsumptionRatesFilterViewModel
    {
        public HeatEnergyConsumptionRatesFilterViewModel() { }

        public HeatEnergyConsumptionRatesFilterViewModel(string? organization, string? productType, int? quantity, 
            int? quarter, int? year)
        {
            Organization = organization;
            ProductType = productType;
            Quantity = quantity;
            Quarter = quarter;
            Year = year;
        }

        public string? Organization { get; set; }

        public string? ProductType { get; set; }

        public int? Quantity { get; set; }

        public int? Quarter { get; set; }

        public int? Year { get; set; }
    }
}