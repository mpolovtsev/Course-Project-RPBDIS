namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ProductsTypesFilterViewModel
    {
        public ProductsTypesFilterViewModel() { }

        public ProductsTypesFilterViewModel(string? code, string? name, string? unit)
        {
            Code = code;
            Name = name;
            Unit = unit;
        }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Unit { get; set; }
    }
}