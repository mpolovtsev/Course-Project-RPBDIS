namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ChiefPowerEngineersFilterViewModel
    {
        public ChiefPowerEngineersFilterViewModel() { }

        public ChiefPowerEngineersFilterViewModel(string? name, string? surname, string? middleName, 
            string? organization)
        {
            Name = name;
            Surname = surname;
            MiddleName = middleName;
            Organization = organization;
        }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? MiddleName {  get; set; }

        public string? Organization { get; set; }
    }
}