namespace HeatEnergyConsumption.Models
{
    public partial class HeatEnergyConsumptionRate
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int ProductTypeId { get; set; }
        public float Quantity { get; set; }
        public DateTime Date { get; set; }

        public virtual Organization Organization { get; set; } = null!;
        public virtual ProductsType ProductType { get; set; } = null!;
    }
}
