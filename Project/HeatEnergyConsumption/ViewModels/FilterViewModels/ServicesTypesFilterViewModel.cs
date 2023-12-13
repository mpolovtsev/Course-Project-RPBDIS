namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ServicesTypesFilterViewModel
    {
        public ServicesTypesFilterViewModel() { }

        public ServicesTypesFilterViewModel(string? code, string? name, string? unit)
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