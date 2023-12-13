using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Extensions
{
    public static class FilterExtensions
    {
        public static IEnumerable<ChiefPowerEngineer> Filter(this IEnumerable<ChiefPowerEngineer> chiefPowerEngineers,
            string? name, string? surname, string? middleName, string? organization)
        {
            return chiefPowerEngineers.Where(chiefPowerEngineer => chiefPowerEngineer.Name.Contains(name ?? "") &&
                chiefPowerEngineer.Surname.Contains(surname ?? "") &&
                (chiefPowerEngineer.MiddleName != null ? chiefPowerEngineer.MiddleName.Contains(middleName ?? "") : false) &&
                chiefPowerEngineer.Organization.Name.Contains(organization ?? ""));
        }

        public static IEnumerable<HeatEnergyConsumptionRate> Filter(this IEnumerable<HeatEnergyConsumptionRate> heatEnergyConsumptionRates, 
            string? organization, string? productType, int? quantity, int? quarter, int? year)
        {
            heatEnergyConsumptionRates = heatEnergyConsumptionRates.Where(heatEnergyConsumptionRate => 
                heatEnergyConsumptionRate.Organization.Name.Contains(organization ?? "") &&
                heatEnergyConsumptionRate.ProductType.Name.Contains(productType ?? ""));

            if (quantity != null)
                heatEnergyConsumptionRates = heatEnergyConsumptionRates.Where(heatEnergyConsumptionRate => 
                    heatEnergyConsumptionRate.Quantity < quantity);

            if (quarter != null)
                heatEnergyConsumptionRates = heatEnergyConsumptionRates.Where(heatEnergyConsumptionRate => 
                    (heatEnergyConsumptionRate.Date.GetQuarter() == quarter));

            if (year != null)
                heatEnergyConsumptionRates = heatEnergyConsumptionRates.Where(heatEnergyConsumptionRate => 
                    heatEnergyConsumptionRate.Date.Year == year);

            return heatEnergyConsumptionRates;
        }

        public static IEnumerable<Manager> Filter(this IEnumerable<Manager> managers, string? name,
            string? surname, string? middleName)
        {
            return managers.Where(manager => manager.Name.Contains(name ?? "") &&
                manager.Surname.Contains(surname ?? "") &&
                manager.MiddleName != null ? manager.MiddleName.Contains(middleName ?? "") : false);
        }

        public static IEnumerable<Organization> Filter(this IEnumerable<Organization> organizations, 
            string? name, string? ownershipForm, string? address, string? manager)
        {
            return organizations.Where(organization => organization.Name.Contains(name ?? "") &&
                organization.OwnershipForm.Name.Contains(ownershipForm ?? "") &&
                organization.Address.Contains(address ?? "") &&
                organization.Manager != null ? organization.Manager.Surname.Contains(manager ?? "") : false);
        }

        public static IEnumerable<OwnershipForm> Filter(this IEnumerable<OwnershipForm> ownershipForms,
            string? name) 
        {
            return ownershipForms.Where(ownershipForm => ownershipForm.Name.Contains(name ?? ""));
        }

        public static IEnumerable<ProducedProduct> Filter(this IEnumerable<ProducedProduct> producedProducts, 
            string? organization, string? productType, int? productQuantity, int? heatEnergyQuantity, int? quarter, int? year)
        {
            producedProducts = producedProducts.Where(producedProduct => 
                producedProduct.Organization.Name.Contains(organization ?? "") &&
                producedProduct.ProductType.Name.Contains(productType ?? ""));

            if (productQuantity != null)
                producedProducts = producedProducts.Where(producedProduct => 
                    producedProduct.ProductQuantity < productQuantity);

            if (heatEnergyQuantity != null)
                producedProducts = producedProducts.Where(producedProduct => 
                    producedProduct.HeatEnergyQuantity < heatEnergyQuantity);

            if (quarter != null)
                producedProducts = producedProducts.Where(producedProduct => 
                    (producedProduct.Date.Month + 2) / 3 == quarter);

            if (year != null)
                producedProducts = producedProducts.Where(producedProduct => 
                    producedProduct.Date.Year == year);

            return producedProducts;
        }

        public static IEnumerable<ProductsType> Filter(this IEnumerable<ProductsType> productsTypes, 
            string? code, string? name, string? unit)
        {
            return productsTypes.Where(productsType => productsType.Code.Contains(code ?? "") &&
                productsType.Name.Contains(name ?? "") &&
                productsType.Unit.Contains(unit ?? ""));
        }

        public static IEnumerable<ProvidedService> Filter(this IEnumerable<ProvidedService> providedServices, 
            string? organization, string? serviceType, int? quantity, int? quarter, int? year)
        {
            providedServices = providedServices.Where(providedService => 
                providedService.Organization.Name.Contains(organization ?? "") &&
                providedService.ServiceType.Name.Contains(serviceType ?? ""));

            if (quantity != null)
                providedServices = providedServices.Where(providedService => 
                    providedService.Quantity < quantity);

            if (quarter != null)
                providedServices = providedServices.Where(providedService => 
                    (providedService.Date.Month + 2) / 3 == quarter);

            if (year != null)
                providedServices = providedServices.Where(providedService => 
                    providedService.Date.Year == year);

            return providedServices;
        }

        public static IEnumerable<ServicesType> Filter(this IEnumerable<ServicesType> servicesTypes, 
            string? code, string? name, string? unit)
        {
            return servicesTypes.Where(servicesType => servicesType.Code.Contains(code ?? "") &&
                servicesType.Name.Contains(name ?? "") &&
                servicesType.Unit.Contains(unit ?? ""));
        }

        public static IEnumerable<ComparisonHeatEnergyAmount> Filter(this IEnumerable<ComparisonHeatEnergyAmount> comparisonsHeatEnergyAmount,
            string? organization, string? productType, double? actualHeatEnergyConsumption, double? normalizedHeatEnergyConsumption,
            int? quarter, int? year)
        {
            comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Where(heatEnergyConsumption =>
                heatEnergyConsumption.Organization.Contains(organization ?? "") &&
                heatEnergyConsumption.ProductType.Contains(productType ?? ""));

            if (actualHeatEnergyConsumption != null)
                comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Where(heatEnergyConsumption =>
                    heatEnergyConsumption.ActualHeatEnergyConsumption < actualHeatEnergyConsumption);

            if (normalizedHeatEnergyConsumption != null)
                comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Where(heatEnergyConsumption =>
                    heatEnergyConsumption.NormalizedHeatEnergyConsumption < normalizedHeatEnergyConsumption);

            if (quarter != null)
                comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Where(heatEnergyConsumption =>
                    heatEnergyConsumption.Quarter == quarter);

            if (year != null)
                comparisonsHeatEnergyAmount = comparisonsHeatEnergyAmount.Where(heatEnergyConsumption =>
                    heatEnergyConsumption.Year == year);

            return comparisonsHeatEnergyAmount;
        }

        public static IEnumerable<ViolatorOrganization> Filter(this IEnumerable<ViolatorOrganization> violatorsOrganizations, 
            string? organization, string? productType, double? difference, int? quarter, int? year)
        {
            violatorsOrganizations = violatorsOrganizations.Where(violatorOrganization => 
                violatorOrganization.Organization.Contains(organization ?? "") &&
                violatorOrganization.ProductType.Contains(productType ?? ""));

            if (difference != null)
                violatorsOrganizations = violatorsOrganizations.Where(violatorOrganization =>
                    violatorOrganization.Difference < difference);

            if (quarter != null)
                violatorsOrganizations = violatorsOrganizations.Where(violatorOrganization =>
                    violatorOrganization.Quarter == quarter);

            if (year != null)
                violatorsOrganizations = violatorsOrganizations.Where(violatorOrganization => 
                    violatorOrganization.Year == year);

            return violatorsOrganizations;
        }

        public static IEnumerable<ViolatorProductsType> Filter(this IEnumerable<ViolatorProductsType> violatorsProductsTypes,
            string? code, string? type, string? organization, double? exceeding, int? quarter, int? year)
        {
            violatorsProductsTypes = violatorsProductsTypes.Where(violatorProductsType =>
                violatorProductsType.Code.Contains(code ?? "") &&
                violatorProductsType.Type.Contains(type ?? "") &&
                violatorProductsType.Organization.Contains(organization ?? ""));

            if (exceeding != null)
                violatorsProductsTypes = violatorsProductsTypes.Where(violatorProductsType =>
                    violatorProductsType.Exceeding < exceeding);

            if (quarter != null)
                violatorsProductsTypes = violatorsProductsTypes.Where(violatorProductsType =>
                    violatorProductsType.Quarter == quarter);

            if (year != null)
                violatorsProductsTypes = violatorsProductsTypes.Where(violatorProductsType =>
                    violatorProductsType.Year == year);

            return violatorsProductsTypes;
        }
    }
}