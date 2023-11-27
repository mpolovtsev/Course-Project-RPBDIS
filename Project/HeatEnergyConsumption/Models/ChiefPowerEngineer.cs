namespace HeatEnergyConsumption.Models
{
    public partial class ChiefPowerEngineer
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public int OrganizationId { get; set; }

        public virtual Organization Organization { get; set; } = null!;
    }
}