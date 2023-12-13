namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
    public class OrganizationsFilterViewModel
    {
        public OrganizationsFilterViewModel() { }

        public OrganizationsFilterViewModel(string? name, string? ownershipForm, string? address, string? manager) 
        {
            Name = name;
            OwnershipForm = ownershipForm;
            Address = address;
            Manager = manager;
        }

        public string? Name {  get; set; }

        public string? OwnershipForm { get; set; }

        public string? Address { get; set; }

        public string? Manager { get; set; }
    }
}