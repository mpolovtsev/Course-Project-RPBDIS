using System.ComponentModel.DataAnnotations;

namespace HeatEnergyConsumption.Models
{
    public class ViolatorOrganization
    {
        public int Id { get; set; }

        [Display(Name = "РАЗНИЦА")]
        public double Difference { get; set; }

        [Display(Name = "ОРГАНИЗАЦИЯ")]
        public string Organization { get; set; }

        [Display(Name = "ТИП ПРОДУКЦИИ")]
        public string ProductType { get; set; }

        [Display(Name = "КВАРТАЛ")]
        public int Quarter { get; set; }

        [Display(Name = "ГОД")]
        public int Year { get; set; }
    }
}