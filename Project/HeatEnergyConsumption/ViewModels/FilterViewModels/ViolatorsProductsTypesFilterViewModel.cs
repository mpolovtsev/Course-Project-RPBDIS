namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ViolatorsProductsTypesFilterViewModel
    {
        public ViolatorsProductsTypesFilterViewModel() { }

        public ViolatorsProductsTypesFilterViewModel(string? code, string? type, string? organization, double? exceeding, 
            int? quarter, int? year)
        {
            Code = code;
            Type = type;
            Organization = organization;
            Exceeding = exceeding;
            Quarter = quarter;
            Year = year;
        }

        public string? Code { get; set; }

        public string? Type { get; set; }

        public string? Organization { get; set; }

        public double? Exceeding { get; set; }

        public int? Quarter { get; set; }

        public int? Year { get; set; }
    }
}