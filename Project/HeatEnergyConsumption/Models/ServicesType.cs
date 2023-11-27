namespace HeatEnergyConsumption.Models
{
    public partial class ServicesType
    {
        public ServicesType()
        {
            ProvidedServices = new HashSet<ProvidedService>();
        }

        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;

        public virtual ICollection<ProvidedService> ProvidedServices { get; set; }
    }
}