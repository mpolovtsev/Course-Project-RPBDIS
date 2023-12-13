using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ComparisonsHeatEnergyAmountFilterViewModel
    {
        public ComparisonsHeatEnergyAmountFilterViewModel() { }

        public ComparisonsHeatEnergyAmountFilterViewModel(string? organization, string? productType,
            double? actualHeatEnergyConsumption, double? normalizedHeatEnergyConsumption, int? quarter, int? year)
        {
            Organization = organization;
            ProductType = productType;
            ActualHeatEnergyConsumption = actualHeatEnergyConsumption;
            NormalizedHeatEnergyConsumption = normalizedHeatEnergyConsumption;
            Quarter = quarter;
            Year = year;
        }

        public string? Organization {  get; set; }

        public string? ProductType { get; set; }

        public double? ActualHeatEnergyConsumption { get; set; }

        public double? NormalizedHeatEnergyConsumption { get; set; }

        public int? Quarter { get; set; }

        public int? Year { get; set; }
    }
}