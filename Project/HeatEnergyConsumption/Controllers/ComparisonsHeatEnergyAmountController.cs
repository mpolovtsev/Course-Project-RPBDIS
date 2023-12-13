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
    public class ComparisonsHeatEnergyAmountController : Controller
    {
        readonly HeatEnergyConsumptionContext dbContext;
        readonly int pageSize;

        public ComparisonsHeatEnergyAmountController(HeatEnergyConsumptionContext dbContext, IConfiguration config)
        {
            this.dbContext = dbContext;
            pageSize = int.Parse(config["Parameters:PageSize"]);
        }

        [Authorize]
        public IActionResult Index(ComparisonsHeatEnergyAmountFilterViewModel filterViewModel,
            ComparisonsHeatEnergyAmountSortState sortOrder = ComparisonsHeatEnergyAmountSortState.OrganizationAsc, int page = 1)
        {
            IEnumerable<ComparisonHeatEnergyAmount> comparisonsHeatEnergyAmount =
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
                    ActualHeatEnergyConsumptionPerUnit = producedProduct.HeatEnergyQuantity / producedProduct.ProductQuantity,
                    NormalizedHeatEnergyConsumptionPerUnit = heatEnergyConsumptionRate.Quantity / producedProduct.ProductQuantity,
                    Quarter = (producedProduct.Date.Month + 2) / 3,
                    Year = producedProduct.Date.Year
                }
                by new
                {
                    OrganizationName = organization.Name,
                    ProductTypeName = productType.Name,
                    Quarter = (producedProduct.Date.Month + 2) / 3,
                    Year = producedProduct.Date.Year
                }
                into groupedData
                select new ComparisonHeatEnergyAmount
                {
                    Organization = groupedData.Key.OrganizationName,
                    ProductType = groupedData.Key.ProductTypeName,
                    ActualHeatEnergyConsumption = groupedData.Sum(record => record.ActualHeatEnergyConsumptionPerUnit),
                    NormalizedHeatEnergyConsumption = groupedData.Sum(record => record.NormalizedHeatEnergyConsumptionPerUnit),
                    Quarter = groupedData.Key.Quarter,
                    Year = groupedData.Key.Year,
                };

            // Фильтрация
            if (HttpContext.Request.Method == "GET")
            {
                HttpContext.Request.Cookies.TryGetValue("ComparisonHeatEnergyAmountOrganization", out string? organizationCookie);
                HttpContext.Request.Cookies.TryGetValue("ComparisonHeatEnergyAmountProductType", out string? productTypeCookie);
                HttpContext.Request.Cookies.TryGetValue("ComparisonHeatEnergyAmountActualHeatEnergyConsumption", out string? actualHeatEnergyConsumptionCookie);
                HttpContext.Request.Cookies.TryGetValue("ComparisonHeatEnergyAmountNormalizedHeatEnergyConsumption", out string? normalizedHeatEnergyConsumptionCookie);
                HttpContext.Request.Cookies.TryGetValue("ComparisonHeatEnergyAmountQuarter", out string? quarterCookie);
                HttpContext.Request.Cookies.TryGetValue("ComparisonHeatEnergyAmountYear", out string? yearCookie);

                if (!(string.IsNullOrEmpty(organizationCookie) && string.IsNullOrEmpty(productTypeCookie) && string.IsNullOrEmpty(actualHeatEnergyConsumptionCookie) &&
                    string.IsNullOrEmpty(normalizedHeatEnergyConsumptionCookie) && string.IsNullOrEmpty(quarterCookie) && string.IsNullOrEmpty(yearCookie)))
                {
                    filterViewModel.Organization = organizationCookie;
                    filterViewModel.ProductType = productTypeCookie;
                    filterViewModel.ActualHeatEnergyConsumption = 
                        double.TryParse(actualHeatEnergyConsumptionCookie, out double actualHeatEnergyConsumption) ? actualHeatEnergyConsumption : null;
                    filterViewModel.NormalizedHeatEnergyConsumption =
                        double.TryParse(normalizedHeatEnergyConsumptionCookie, out double normalizedHeatEnergyConsumption) ? normalizedHeatEnergyConsumption : null;
                    filterViewModel.Quarter = int.TryParse(quarterCookie, out int quarter) ? quarter : null;
                    filterViewModel.Year = int.TryParse(yearCookie, out int year) ? year : null;
                    comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Filter(filterViewModel.Organization, filterViewModel.ProductType, 
                        filterViewModel.ActualHeatEnergyConsumption, filterViewModel.NormalizedHeatEnergyConsumption, filterViewModel.Quarter, filterViewModel.Year);
                }
            }
            else if (HttpContext.Request.Method == "POST")
            {
                if (!(string.IsNullOrEmpty(filterViewModel.Organization) && string.IsNullOrEmpty(filterViewModel.ProductType) && 
                    filterViewModel.ActualHeatEnergyConsumption != null && filterViewModel.NormalizedHeatEnergyConsumption != null && filterViewModel.Quarter != null && 
                    filterViewModel.Year != null))
                {
                    comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Filter(filterViewModel.Organization, filterViewModel.ProductType,
                        filterViewModel.ActualHeatEnergyConsumption, filterViewModel.NormalizedHeatEnergyConsumption, filterViewModel.Quarter, filterViewModel.Year);

                    if (!string.IsNullOrEmpty(filterViewModel.Organization))
                        HttpContext.Response.Cookies.Append("ComparisonHeatEnergyAmountOrganization", filterViewModel.Organization);
                    else
                        HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountOrganization");

                    if (!string.IsNullOrEmpty(filterViewModel.ProductType))
                        HttpContext.Response.Cookies.Append("ComparisonHeatEnergyAmountProductType", filterViewModel.ProductType);
                    else
                        HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountProductType");

                    if (filterViewModel.ActualHeatEnergyConsumption != null)
                        HttpContext.Response.Cookies.Append("ComparisonHeatEnergyAmountActualHeatEnergyConsumption", filterViewModel.ActualHeatEnergyConsumption.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountActualHeatEnergyConsumption");

                    if (filterViewModel.NormalizedHeatEnergyConsumption != null)
                        HttpContext.Response.Cookies.Append("ComparisonHeatEnergyAmountNormalizedHeatEnergyConsumption", filterViewModel.NormalizedHeatEnergyConsumption.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountNormalizedHeatEnergyConsumption");

                    if (filterViewModel.Quarter != null)
                        HttpContext.Response.Cookies.Append("ComparisonHeatEnergyAmountQuarter", filterViewModel.Quarter.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountQuarter");

                    if (filterViewModel.Year != null)
                        HttpContext.Response.Cookies.Append("ComparisonHeatEnergyAmountYear", filterViewModel.Year.ToString());
                    else
                        HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountYear");
                }
                else
                {
                    HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountOrganization");
                    HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountProductType");
                    HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountActualHeatEnergyConsumption");
                    HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountNormalizedHeatEnergyConsumption");
                    HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountQuarter");
                    HttpContext.Response.Cookies.Delete("ComparisonHeatEnergyAmountYear");
                }
            }

            // Сортировка
            comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Sort(sortOrder);
            ComparisonsHeatEnergyAmountSortViewModel sortViewModel = new ComparisonsHeatEnergyAmountSortViewModel(sortOrder);

            // Пагинация
            int count = comparisonsHeatEnergyAmount.Count();
            comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Paginate(page, pageSize);
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            // Формирование модели для передачи представлению
            ComparisonsHeatEnergyAmountViewModel model = new ComparisonsHeatEnergyAmountViewModel()
            {
                ComparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount,
                FilterViewModel = filterViewModel,
                SortViewModel = sortViewModel,
                PageViewModel = pageViewModel
            };

            return View(model);
        }
    }
}