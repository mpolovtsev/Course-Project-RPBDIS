namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ProducedProductsFilterViewModel
    {
        public ProducedProductsFilterViewModel() { }

        public ProducedProductsFilterViewModel(string organization, string productType, int? productQuantity, int? heatEnergyQuantity, 
            int? quarter, int? year)
        {
            Organization = organization;
            ProductType = productType;
            ProductQuantity = productQuantity;
            HeatEnergyQuantity = heatEnergyQuantity;
            Quarter = quarter;
            Year = year;
        }

        public string? Organization { get; set; }

        public string? ProductType { get; set; }

        public int? ProductQuantity { get; set; }

        public int? HeatEnergyQuantity { get; set; }

        public int? Quarter { get; set; }

        public int? Year { get; set; }
    }
}