using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
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
    public class ViolatorsOrganizationsController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ViolatorsOrganizationsController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ViolatorsOrganizationsFilterViewModel filterViewModel,
            ViolatorsOrganizationsSortState sortOrder = ViolatorsOrganizationsSortState.OrganizationAsc, int page = 1)
        {
            IEnumerable<ViolatorOrganization> violatorsOrganizations = 
                from producedProduct in dbContext.ProducedProducts
                join organization in dbContext.Organizations on producedProduct.OrganizationId equals organization.Id
                join productType in dbContext.ProductsTypes on producedProduct.ProductTypeId equals productType.Id
                join heatEnergyConsumptionRate in dbContext.HeatEnergyConsumptionRates on 
                    new { producedProduct.OrganizationId, producedProduct.ProductTypeId, producedProduct.Date } equals 
                    new { heatEnergyConsumptionRate.OrganizationId, heatEnergyConsumptionRate.ProductTypeId, heatEnergyConsumptionRate.Date }
                group new
                {
                    OrganizationName = organization.Name,
                    ProductTypeName = productType.Name,
                    Quarter = (producedProduct.Date.Month + 2) / 3,
                    Year = producedProduct.Date.Year,
                    ActualHeatEnergyConsumptionPerUnit = producedProduct.HeatEnergyQuantity / producedProduct.ProductQuantity,
                    NormalizedHeatEnergyConsumptionPerUnit = heatEnergyConsumptionRate.Quantity / producedProduct.ProductQuantity
                } 
                by new
                {
                    OrganizationName = organization.Name,
                    ProductTypeName = productType.Name,
                    Quarter = (producedProduct.Date.Month + 2) / 3,
                    Year = producedProduct.Date.Year
                } 
                into groupedData
                select new ViolatorOrganization
                {
                    Organization = groupedData.Key.OrganizationName,
                    ProductType = groupedData.Key.ProductTypeName,
                    Difference = groupedData.Sum(x => x.ActualHeatEnergyConsumptionPerUnit) - 
                        groupedData.Sum(x => x.NormalizedHeatEnergyConsumptionPerUnit),
                    Quarter = groupedData.Key.Quarter,
                    Year = groupedData.Key.Year,
                };

            if (violatorsOrganizations == null)
                return Problem("Записи не найдены.");

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ViolatorOrganizationOrganization", out string? organizationCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorOrganizationProductType", out string? productTypeCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorOrganizationDifference", out string? differenceCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorOrganizationQuarter", out string? quarterCookie);
                HttpContext.Request.Cookies.TryGetValue("ViolatorOrganizationYear", out string? yearCookie);

                if (!(string.IsNullOrEmpty(organizationCookie) && string.IsNullOrEmpty(productTypeCookie) && string.IsNullOrEmpty(differenceCookie) &&
                    string.IsNullOrEmpty(quarterCookie) && string.IsNullOrEmpty(yearCookie)))
                {
                    filterViewModel.Organization = organizationCookie;
                    filterViewModel.ProductType = productTypeCookie;
                    filterViewModel.Difference = double.TryParse(differenceCookie, out double difference) ? difference : null;
                    filterViewModel.Quarter = int.TryParse(quarterCookie, out int quarter) ? quarter : null;
                    filterViewModel.Year = int.TryParse(yearCookie, out int year) ? year : null;
                    violatorsOrganizations = violatorsOrganizations.Filter(filterViewModel.Organization, filterViewModel.ProductType, 
                        filterViewModel.Difference, filterViewModel.Quarter, filterViewModel.Year);
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Organization) && string.IsNullOrEmpty(filterViewModel.ProductType) && 
                    filterViewModel.Difference != null && filterViewModel.Quarter != null && filterViewModel.Year != null))
                {
                    violatorsOrganizations = violatorsOrganizations.Filter(filterViewModel.Organization, filterViewModel.ProductType, 
                        filterViewModel.Difference, filterViewModel.Quarter, filterViewModel.Year);

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("ViolatorOrganizationOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorOrganizationOrganization");

                    if (!string.IsNullOrEmpty(filterViewModel.ProductType))
                        HttpContext.Response.Cookies.Append("ViolatorOrganizationProductType", filterViewModel.ProductType);
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorOrganizationProductType");

                    if (filterViewModel.Difference != null)
                        HttpContext.Response.Cookies.Append("ViolatorOrganizationDifference", filterViewModel.Difference.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorOrganizationDifference");

                    if (filterViewModel.Quarter != null)
                        HttpContext.Response.Cookies.Append("ViolatorOrganizationQuarter", filterViewModel.Quarter.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorOrganizationQuarter");

                    if (filterViewModel.Year != null)
                        HttpContext.Response.Cookies.Append("ViolatorOrganizationYear", filterViewModel.Year.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ViolatorOrganizationYear");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ViolatorOrganizationOrganization");
                    HttpContext.Response.Cookies.Delete("ViolatorOrganizationProductType");
                    HttpContext.Response.Cookies.Delete("ViolatorOrganizationDifference");
                    HttpContext.Response.Cookies.Delete("ViolatorOrganizationQuarter");
                    HttpContext.Response.Cookies.Delete("ViolatorOrganizationYear");
                }
            }

            // Сортировка
            violatorsOrganizations = violatorsOrganizations.Sort(sortOrder);
            ViolatorsOrganizationsSortViewModel sortViewModel = new ViolatorsOrganizationsSortViewModel(sortOrder);

            // Пагинация
            int count = violatorsOrganizations.Count();
            violatorsOrganizations = violatorsOrganizations.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ViolatorsOrganizationsViewModel model = new ViolatorsOrganizationsViewModel()
            {
                ViolatorsOrganizations = violatorsOrganizations,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }
    }
}