using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.Extensions
{
    public static class SortExtensions
    {
        public static IEnumerable<ChiefPowerEngineer> Sort(this IEnumerable<ChiefPowerEngineer> chiefPowerEngineers,
            ChiefPowerEngineersSortState sortOrder = ChiefPowerEngineersSortState.NameAsc)
        {
            return sortOrder switch
            {
                ChiefPowerEngineersSortState.NameDesc =>
                    chiefPowerEngineers.OrderByDescending(chiefPowerEngineer => chiefPowerEngineer.Name),
                ChiefPowerEngineersSortState.SurnameAsc =>
                    chiefPowerEngineers.OrderBy(chiefPowerEngineer => chiefPowerEngineer.Surname),
                ChiefPowerEngineersSortState.SurnameDesc =>
                    chiefPowerEngineers.OrderByDescending(chiefPowerEngineer => chiefPowerEngineer.Surname),
                ChiefPowerEngineersSortState.MiddleNameAsc =>
                    chiefPowerEngineers.OrderBy(chiefPowerEngineer => chiefPowerEngineer.MiddleName),
                ChiefPowerEngineersSortState.MiddleNameDesc =>
                    chiefPowerEngineers.OrderByDescending(chiefPowerEngineer => chiefPowerEngineer.MiddleName),
                ChiefPowerEngineersSortState.OrganizationAsc =>
                    chiefPowerEngineers.OrderBy(chiefPowerEngineer => chiefPowerEngineer.Organization.Name),
                ChiefPowerEngineersSortState.OrganizationDesc =>
                    chiefPowerEngineers.OrderByDescending(chiefPowerEngineer => chiefPowerEngineer.Organization.Name),
                _ => chiefPowerEngineers.OrderBy(chiefPowerEngineer => chiefPowerEngineer.Name)
            };
        }

        public static IEnumerable<HeatEnergyConsumptionRate> Sort(this IEnumerable<HeatEnergyConsumptionRate> heatEnergyConsumptionRates,
            HeatEnergyConsumptionRatesSortState sortOrder = HeatEnergyConsumptionRatesSortState.OrganizationAsc)
        {
            return sortOrder switch
            {
                HeatEnergyConsumptionRatesSortState.OrganizationDesc => 
                    heatEnergyConsumptionRates.OrderByDescending(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Organization.Name)
                    .ThenBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                HeatEnergyConsumptionRatesSortState.ProductTypeAsc => 
                    heatEnergyConsumptionRates.OrderBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.ProductType.Name)
                    .ThenBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                HeatEnergyConsumptionRatesSortState.ProductTypeDesc => 
                    heatEnergyConsumptionRates.OrderByDescending(heatEnergyConsumptionRate => heatEnergyConsumptionRate.ProductType.Name)
                    .ThenBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                HeatEnergyConsumptionRatesSortState.QuantityAsc => 
                    heatEnergyConsumptionRates.OrderBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Quantity)
                    .ThenBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                HeatEnergyConsumptionRatesSortState.QuantityDesc => 
                    heatEnergyConsumptionRates.OrderByDescending(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Quantity)
                    .ThenBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                HeatEnergyConsumptionRatesSortState.YearAsc => 
                    heatEnergyConsumptionRates.OrderBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                HeatEnergyConsumptionRatesSortState.YearDesc => 
                    heatEnergyConsumptionRates.OrderByDescending(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year),
                _ => heatEnergyConsumptionRates.OrderBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Organization.Name)
                    .ThenBy(heatEnergyConsumptionRate => heatEnergyConsumptionRate.Date.Year)
            };
        }

        public static IEnumerable<Manager> Sort(this IEnumerable<Manager> managers,
            ManagersSortState sortOrder = ManagersSortState.NameAsc)
        {
            return sortOrder switch
            {
                ManagersSortState.NameDesc =>
                    managers.OrderByDescending(manager => manager.Name),
                ManagersSortState.SurnameAsc =>
                    managers.OrderBy(manager => manager.Surname),
                ManagersSortState.SurnameDesc =>
                    managers.OrderByDescending(manager => manager.Surname),
                ManagersSortState.MiddleNameAsc =>
                    managers.OrderBy(manager => manager.MiddleName),
                ManagersSortState.MiddleNameDesc =>
                    managers.OrderByDescending(manager => manager.MiddleName),
                _ => managers.OrderBy(manager => manager.Name)
            };
        }

        public static IEnumerable<Organization> Sort(this IEnumerable<Organization> organizations,
            OrganizationsSortState sortOrder = OrganizationsSortState.NameAsc)
        {
            return sortOrder switch
            {
                OrganizationsSortState.NameDesc => 
                    organizations.OrderByDescending(organization => organization.Name),
                OrganizationsSortState.OwnershipFormAsc => 
                    organizations.OrderBy(organization => organization.OwnershipForm.Name),
                OrganizationsSortState.OwnershipFormDesc => 
                    organizations.OrderByDescending(organization => organization.OwnershipForm.Name),
                OrganizationsSortState.ManagerAsc => 
                    organizations.OrderBy(organization => organization.Manager?.Surname),
                OrganizationsSortState.ManagerDesc => 
                    organizations.OrderByDescending(organization => organization.Manager?.Surname),
                _ => organizations.OrderBy(organization => organization.Name)
            };
        }

        public static IEnumerable<OwnershipForm> Sort(this IEnumerable<OwnershipForm> ownershipForms, 
            OwnershipFormsSortState sortOrder = OwnershipFormsSortState.NameAsc)
        {
            return sortOrder switch
            {
                OwnershipFormsSortState.NameDesc => 
                    ownershipForms.OrderByDescending(ownershipForm => ownershipForm.Name),
                _ => ownershipForms.OrderBy(ownershipForm => ownershipForm.Name)
            };
        }

        public static IEnumerable<ProducedProduct> Sort(this IEnumerable<ProducedProduct> producedProducts,
            ProducedProductsSortState sortOrder = ProducedProductsSortState.OrganizationAsc)
        {
            return sortOrder switch
            {
                ProducedProductsSortState.OrganizationDesc => 
                    producedProducts.OrderByDescending(producedProduct => producedProduct.Organization.Name)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.ProductTypeAsc => 
                    producedProducts.OrderBy(producedProduct => producedProduct.ProductType.Name)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.ProductTypeDesc => 
                    producedProducts.OrderByDescending(producedProduct => producedProduct.ProductType.Name)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.ProductQuantityAsc => 
                    producedProducts.OrderBy(producedProduct => producedProduct.ProductQuantity)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.ProductQuantityDesc => 
                    producedProducts.OrderByDescending(producedProduct => producedProduct.ProductQuantity)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.HeatEnergyQuantityAsc => 
                    producedProducts.OrderBy(producedProduct => producedProduct.HeatEnergyQuantity)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.HeatEnergyQuantityDesc => 
                    producedProducts.OrderByDescending(producedProduct => producedProduct.HeatEnergyQuantity)
                    .ThenBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.YearAsc => 
                    producedProducts.OrderBy(producedProduct => producedProduct.Date.Year),
                ProducedProductsSortState.YearDesc => 
                    producedProducts.OrderByDescending(producedProduct => producedProduct.Date.Year),
                _ => producedProducts.OrderBy(producedProduct => producedProduct.Organization.Name)
                    .ThenBy(producedProduct => producedProduct.Date.Year)
            };
        }

        public static IEnumerable<ProductsType> Sort(this IEnumerable<ProductsType> productsTypes,
            ProductsTypesSortState sortOrder = ProductsTypesSortState.CodeAsc)
        {
            return sortOrder switch
            {
                ProductsTypesSortState.CodeDesc => 
                    productsTypes.OrderByDescending(productsType => productsType.Code),
                ProductsTypesSortState.NameAsc => 
                    productsTypes.OrderBy(productsType => productsType.Name),
                ProductsTypesSortState.NameDesc => 
                    productsTypes.OrderByDescending(productsType => productsType.Name),
                _ => productsTypes.OrderBy(productsType => productsType.Code)
            };
        }

        public static IEnumerable<ProvidedService> Sort(this IEnumerable<ProvidedService> providedServices,
            ProvidedServicesSortState sortOrder = ProvidedServicesSortState.OrganizationAsc)
        {
            return sortOrder switch
            {
                ProvidedServicesSortState.OrganizationDesc => 
                    providedServices.OrderByDescending(providedService => providedService.Organization.Name)
                    .ThenBy(providedService => providedService.Date.Year),
                ProvidedServicesSortState.ServiceTypeAsc => 
                    providedServices.OrderBy(providedService => providedService.ServiceType.Name)
                    .ThenBy(providedService => providedService.Date.Year),
                ProvidedServicesSortState.ServiceTypeDesc => 
                    providedServices.OrderByDescending(providedService => providedService.ServiceType.Name)
                    .ThenBy(providedService => providedService.Date.Year),
                ProvidedServicesSortState.QuantityAsc => 
                    providedServices.OrderBy(providedService => providedService.Quantity)
                    .ThenBy(providedService => providedService.Date.Year),
                ProvidedServicesSortState.QuantityDesc => 
                    providedServices.OrderByDescending(providedService => providedService.Quantity)
                    .ThenBy(providedService => providedService.Date.Year),
                ProvidedServicesSortState.YearAsc => 
                    providedServices.OrderBy(providedService => providedService.Date.Year),
                ProvidedServicesSortState.YearDesc => 
                    providedServices.OrderByDescending(providedService => providedService.Date.Year),
                _ => providedServices.OrderBy(providedService => providedService.Organization.Name)
                    .ThenBy(providedService => providedService.Date.Year)
            };
        }

        public static IEnumerable<ServicesType> Sort(this IEnumerable<ServicesType> servicesTypes,
            ServicesTypesSortState sortOrder = ServicesTypesSortState.CodeAsc)
        {
            return sortOrder switch
            {
                ServicesTypesSortState.CodeDesc => 
                    servicesTypes.OrderByDescending(servicesType => servicesType.Code),
                ServicesTypesSortState.NameAsc => 
                    servicesTypes.OrderBy(servicesType => servicesType.Name),
                ServicesTypesSortState.NameDesc => 
                    servicesTypes.OrderByDescending(servicesType => servicesType.Name),
                _ => servicesTypes.OrderBy(servicesType => servicesType.Code)
            };
        }

        public static IEnumerable<ComparisonHeatEnergyAmount> Sort(this IEnumerable<ComparisonHeatEnergyAmount> comparisonsHeatEnergyAmount,
            ComparisonsHeatEnergyAmountSortState sortOrder = ComparisonsHeatEnergyAmountSortState.OrganizationAsc)
        {
            return sortOrder switch
            {
                ComparisonsHeatEnergyAmountSortState.OrganizationDesc =>
                    comparisonsHeatEnergyAmount.OrderByDescending(heatEnergyConsumption => heatEnergyConsumption.Organization)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.ProductTypeAsc =>
                    comparisonsHeatEnergyAmount.OrderBy(heatEnergyConsumption => heatEnergyConsumption.ProductType)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.ProductTypeDesc =>
                    comparisonsHeatEnergyAmount.OrderByDescending(heatEnergyConsumption => heatEnergyConsumption.ProductType)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.ActualHeatEnergyConsumptionAsc =>
                    comparisonsHeatEnergyAmount.OrderBy(heatEnergyConsumption => heatEnergyConsumption.ActualHeatEnergyConsumption)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.ActualHeatEnergyConsumptionDesc =>
                    comparisonsHeatEnergyAmount.OrderByDescending(heatEnergyConsumption => heatEnergyConsumption.ActualHeatEnergyConsumption)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.NormalizedHeatEnergyConsumptionAsc =>
                    comparisonsHeatEnergyAmount.OrderBy(heatEnergyConsumption => heatEnergyConsumption.NormalizedHeatEnergyConsumption)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.NormalizedHeatEnergyConsumptionDesc =>
                    comparisonsHeatEnergyAmount.OrderByDescending(heatEnergyConsumption => heatEnergyConsumption.NormalizedHeatEnergyConsumption)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.YearAsc =>
                    comparisonsHeatEnergyAmount.OrderBy(heatEnergyConsumption => heatEnergyConsumption.Year),
                ComparisonsHeatEnergyAmountSortState.YearDesc =>
                    comparisonsHeatEnergyAmount.OrderByDescending(heatEnergyConsumption => heatEnergyConsumption.Year),
                _ => comparisonsHeatEnergyAmount.OrderBy(heatEnergyConsumption => heatEnergyConsumption.Organization)
                    .ThenBy(heatEnergyConsumption => heatEnergyConsumption.Year)
            };
        }

        public static IEnumerable<ViolatorOrganization> Sort(this IEnumerable<ViolatorOrganization> violatorsOrganizations,
            ViolatorsOrganizationsSortState sortOrder = ViolatorsOrganizationsSortState.OrganizationAsc)
        {
            return sortOrder switch
            {
                ViolatorsOrganizationsSortState.OrganizationDesc => 
                    violatorsOrganizations.OrderByDescending(violatorOrganization => violatorOrganization.Organization)
                    .ThenBy(violatorOrganization => violatorOrganization.Year),
                ViolatorsOrganizationsSortState.ProductTypeAsc => 
                    violatorsOrganizations.OrderBy(violatorOrganization => violatorOrganization.ProductType)
                    .ThenBy(violatorOrganization => violatorOrganization.Year),
                ViolatorsOrganizationsSortState.ProductTypeDesc => 
                    violatorsOrganizations.OrderByDescending(violatorOrganization => violatorOrganization.ProductType)
                    .ThenBy(violatorOrganization => violatorOrganization.Year),
                ViolatorsOrganizationsSortState.DifferenceAsc => 
                    violatorsOrganizations.OrderBy(violatorOrganization => violatorOrganization.Difference)
                    .ThenBy(violatorOrganization => violatorOrganization.Year),
                ViolatorsOrganizationsSortState.DifferenceDesc => 
                    violatorsOrganizations.OrderByDescending(violatorOrganization => violatorOrganization.Difference)
                    .ThenBy(violatorOrganization => violatorOrganization.Year),
                ViolatorsOrganizationsSortState.YearAsc => 
                    violatorsOrganizations.OrderBy(violatorOrganization => violatorOrganization.Year),
                ViolatorsOrganizationsSortState.YearDesc => 
                    violatorsOrganizations.OrderByDescending(violatorOrganization => violatorOrganization.Year),
                _ => violatorsOrganizations.OrderBy(violatorOrganization => violatorOrganization.Organization)
                    .ThenBy(violatorOrganization => violatorOrganization.Year)
            };
        }

        public static IEnumerable<ViolatorProductsType> Sort(this IEnumerable<ViolatorProductsType> violatorsProductsTypes,
            ViolatorsProductsTypesSortState sortOrder = ViolatorsProductsTypesSortState.CodeAsc)
        {
            return sortOrder switch
            {
                ViolatorsProductsTypesSortState.CodeDesc =>
                    violatorsProductsTypes.OrderByDescending(violatorProductsType => violatorProductsType.Code)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.TypeAsc =>
                    violatorsProductsTypes.OrderBy(violatorProductsType => violatorProductsType.Type)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.TypeDesc =>
                    violatorsProductsTypes.OrderByDescending(violatorProductsType => violatorProductsType.Type)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.OrganizationAsc =>
                    violatorsProductsTypes.OrderBy(violatorProductsType => violatorProductsType.Organization)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.OrganizationDesc =>
                    violatorsProductsTypes.OrderByDescending(violatorProductsType => violatorProductsType.Organization)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.ExceedingAsc =>
                    violatorsProductsTypes.OrderBy(violatorProductsType => violatorProductsType.Exceeding)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.ExceedingDesc =>
                    violatorsProductsTypes.OrderByDescending(violatorProductsType => violatorProductsType.Exceeding)
                    .ThenBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.YearAsc =>
                    violatorsProductsTypes.OrderBy(violatorProductsType => violatorProductsType.Year),
                ViolatorsProductsTypesSortState.YearDesc =>
                    violatorsProductsTypes.OrderByDescending(violatorProductsType => violatorProductsType.Year),
                _ => violatorsProductsTypes.OrderBy(violatorProductsType => violatorProductsType.Code)
                    .ThenBy(violatorProductsType => violatorProductsType.Year)
            };
        }
    }
}