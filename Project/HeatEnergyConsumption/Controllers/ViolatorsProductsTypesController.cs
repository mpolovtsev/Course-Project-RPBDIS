using Microsoft.AspNetCore.Mvc;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.Extensions;
using HeatEnergyConsumption.ViewModels;
using HeatEnergyConsumption.ViewModels.FilterViewModels;
using HeatEnergyConsumption.ViewModels.SortStates;
using HeatEnergyConsumption.ViewModels.SortViewModels;
using HeatEnergyConsumption.ViewModels.PageViewModels;

namespace HeatEnergyConsumption.Controllers
{
    public class ViolatorsProductsTypesController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ViolatorsProductsTypesController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        public IActionResult Index(ViolatorsProductsTypesFilterViewModel filterViewModel,
            ViolatorsProductsTypesSortState sortOrder = ViolatorsProductsTypesSortState.CodeAsc, int page = 1)
        {
            IEnumerable<ViolatorProductsType> violatorsProductsTypes =
                from producedProduct in dbContext.ProducedProducts
                join organization in dbContext.Organizations on producedProduct.OrganizationId equals organization.Id
                join productType in dbContext.ProductsTypes on producedProduct.ProductTypeId equals productType.Id
                join heatEnergyConsumptionRate in dbContext.HeatEnergyConsumptionRates on
                    new { producedProduct.OrganizationId, producedProduct.ProductTypeId, producedProduct.Date } equals
                    new { heatEnergyConsumptionRate.OrganizationId, heatEnergyConsumptionRate.ProductTypeId, heatEnergyConsumptionRate.Date }
                group new
                {
                    Code = productType.Code,
                    Type = productType.Name,
                    OrganizationName = organization.Name,
                    Quarter = (producedProduct.Date.Month + 2) / 3,
                    Year = producedProduct.Date.Year,
                    ActualHeatEnergyConsumptionPerUnit = producedProduct.HeatEnergyQuantity / producedProduct.ProductQuantity,
                    NormalizedHeatEnergyConsumptionPerUnit = heatEnergyConsumptionRate.Quantity / producedProduct.ProductQuantity
                }
                by new
                {
                    Code = productType.Code,
                    Type = productType.Name,
                    OrganizationName = organization.Name,
                    Quarter = (producedProduct.Date.Month + 2) / 3,
                    Year = producedProduct.Date.Year
                }
                into groupedData
                select new ViolatorProductsType
                {
                    Code = groupedData.Key.Code,
                    Type = groupedData.Key.Type,
                    Organization = groupedData.Key.OrganizationName,
                    Exceeding = groupedData.Sum(x => x.ActualHeatEnergyConsumptionPerUnit) -
                        groupedData.Sum(x => x.NormalizedHeatEnergyConsumptionPerUnit),
                    Quarter = groupedData.Key.Quarter,
                    Year = groupedData.Key.Year,
                };

            if (violatorsProductsTypes == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ViolatorProductsTypeCode", out string? codeCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorProductsTypeType", out string? typeCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorProductsTypeOrganization", out string? organizationCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorProductsTypeExceeding", out string? exceedingCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorProductsTypeQuarter", out string? quarterCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorProductsTypeYear", out string? yearCookie);

                if (!(string.IsNullOrEmpty(codeCookie) && string.IsNullOrEmpty(typeCookie) && string.IsNullOrEmpty(organizationCookie) &&
                    string.IsNullOrEmpty(exceedingCookie) && string.IsNullOrEmpty(quarterCookie) && string.IsNullOrEmpty(yearCookie)))
                {
                    filterViewModel.Code = codeCookie;
                    filterViewModel.Type = typeCookie;
                    filterViewModel.Organization = organizationCookie;
                    filterViewModel.Exceeding = double.TryParse(exceedingCookie, out double exceeding) ? exceeding : null;
                    filterViewModel.Quarter = int.TryParse(quarterCookie, out int quarter) ? quarter : null;
                    filterViewModel.Year = int.TryParse(yearCookie, out int year) ? year : null;
                    violatorsProductsTypes = violatorsProductsTypes.Filter(filterViewModel.Code, filterViewModel.Type, filterViewModel.Organization, 
                        filterViewModel.Exceeding, filterViewModel.Quarter, filterViewModel.Year);
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Code) && string.IsNullOrEmpty(filterViewModel.Type) &&
                    string.IsNullOrEmpty(filterViewModel.Organization) && filterViewModel.Exceeding != null && filterViewModel.Quarter != null && filterViewModel.Year != null))
                {
                    violatorsProductsTypes = violatorsProductsTypes.Filter(filterViewModel.Code, filterViewModel.Type, filterViewModel.Organization,
                        filterViewModel.Exceeding, filterViewModel.Quarter, filterViewModel.Year);

                    if (!string.IsNullOrEmpty(filterViewModel.Code))
                        HttpContext.Response.Cookies.Append("ViolatorProductsTypeCode", filterViewModel.Code);
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorProductsTypeCode");

                    if (!string.IsNullOrEmpty(filterViewModel.Type))
                        HttpContext.Response.Cookies.Append("ViolatorProductsTypeType", filterViewModel.Type);
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorProductsTypeType");

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("ViolatorProductsTypeOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorProductsTypeOrganization");

                    if (filterViewModel.Exceeding != null)
                        HttpContext.Response.Cookies.Append("ViolatorProductsTypeExceeding", filterViewModel.Exceeding.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorProductsTypeExceeding");

                    if (filterViewModel.Quarter != null)
                        HttpContext.Response.Cookies.Append("ViolatorProductsTypeQuarter", filterViewModel.Quarter.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorProductsTypeQuarter");

                    if (filterViewModel.Year != null)
                        HttpContext.Response.Cookies.Append("ViolatorProductsTypeYear", filterViewModel.Year.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorProductsTypeYear");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ViolatorProductsTypeOrganization");
                    HttpContext.Response.Cookies.Delete("ViolatorProductsTypeProductType");
                    HttpContext.Response.Cookies.Delete("ViolatorProductsTypeDifference");
                    HttpContext.Response.Cookies.Delete("ViolatorProductsTypeQuarter");
                    HttpContext.Response.Cookies.Delete("ViolatorProductsTypeYear");
                }
            }

            // Сортировка
            violatorsProductsTypes = violatorsProductsTypes.Sort(sortOrder);
            ViolatorsProductsTypesSortViewModel sortViewModel = new ViolatorsProductsTypesSortViewModel(sortOrder);

            // Пагинация
            int count = violatorsProductsTypes.Count();
            violatorsProductsTypes = violatorsProductsTypes.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ViolatorsProductsTypesViewModel model = new ViolatorsProductsTypesViewModel()
            {
                ViolatorsProductsTypes = violatorsProductsTypes,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }
    }
}