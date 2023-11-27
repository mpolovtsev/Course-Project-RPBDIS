namespace HeatEnergyConsumption.Models
{
    public partial class ProducedProduct
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public int ProductTypeId { get; set; }
        public int ProductQuantity { get; set; }
        public float HeatEnergyQuantity { get; set; }
        public DateTime Date { get; set; }

        public virtual Organization Organization { get; set; } = null!;
        public virtual ProductsType ProductType { get; set; } = null!;
    }
}