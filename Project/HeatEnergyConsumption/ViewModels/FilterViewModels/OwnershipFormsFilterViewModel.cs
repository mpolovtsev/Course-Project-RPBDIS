namespace HeatEnergyConsumption.ViewModels.FilterViewModels
{
	public class OwnershipFormsFilterViewModel
	{
		public OwnershipFormsFilterViewModel() { }

		public OwnershipFormsFilterViewModel(string? name) 
		{ 
			Name = name;
		}

		public string? Name { get; set; }
	}
}