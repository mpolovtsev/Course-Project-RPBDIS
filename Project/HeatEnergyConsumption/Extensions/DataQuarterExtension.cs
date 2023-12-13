namespace HeatEnergyConsumption.Extensions
{
    public static class DataQuarterExtension
    {
        public static int GetQuarter(this DateTime date) => (date.Month + 2) / 3;
    }
}