using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public class ViolatorProductsType
    {
        public int Id { get; set; }

        [Display(Name = "КОД ПРОДУКЦИИ")]
        public string Code { get; set; }

        [Display(Name = "ТИП ПРОДУКЦИИ")]
        public string Type { get; set; }

        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public string Organization { get; set; }

        [Display(Name = "ПРЕВЫШЕНИЕ")]
        public double Exceeding { get; set; }

        [Display(Name = "КВАРТАЛ")]
        public int Quarter { get; set; }

        [Display(Name = "ГОД")]
        public int Year { get; set; }
    }
}