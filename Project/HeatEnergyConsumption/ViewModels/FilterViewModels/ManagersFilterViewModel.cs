namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class ManagersFilterViewModel
    {
        public ManagersFilterViewModel() { }

        public ManagersFilterViewModel(string? name, string? surname, string? middleName) 
        {
            Name = name;
            Surname = surname;
            MiddleName = middleName;
        }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? MiddleName { get; set; }
    }
}