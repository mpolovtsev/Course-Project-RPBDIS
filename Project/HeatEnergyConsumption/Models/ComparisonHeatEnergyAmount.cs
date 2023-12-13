using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public class ComparisonHeatEnergyAmount
    {
        public int Id { get; set; }

        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public string Organization { get; set; }

        [Display(Name = "ТИП ПРОДУКЦИИ")]
        public string ProductType { get; set; }

        [Display(Name = "ФАКТИЧЕСКОЕ ПОТРЕБЛЕНИЕ ТЕПЛОЭНЕРГИИ")]
        public double ActualHeatEnergyConsumption { get; set; }

        [Display(Name = "НОРМИРУЕМОЕ ПОТРЕБЛЕНИЕ ТЕПЛОЭНЕРГИИ")]
        public double NormalizedHeatEnergyConsumption { get; set; }

        [Display(Name = "КВАРТАЛ")]
        public int Quarter { get; set; }

        [Display(Name = "ГОД")]
        public int Year { get; set; }
    }
}