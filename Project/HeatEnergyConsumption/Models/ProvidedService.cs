namespace HeatEnergyConsumption.Models
{
    public partial class ProvidedService
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int ServiceTypeId { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }

        public virtual Organization Organization { get; set; } = null!;
        public virtual ServicesType ServiceType { get; set; } = null!;
    }
}