using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Data
{
    public static class DbInitializer
    {
        static Random random = new Random();

        public static void InitializeDb(HeatEnergyConsumptionContext dbContext)
        {
            dbContext.Database.EnsureCreated();

            if (!dbContext.OwnershipForms.Any())
                InitializeOwnershipForms(dbContext);

            if (!dbContext.Managers.Any())
                InitializeManagers(dbContext);

            if (!dbContext.Organizations.Any())
                InitializeOrganizations(dbContext);

            if (!dbContext.ChiefPowerEngineers.Any())
                InitializeChiefPowerEngineers(dbContext);

            if (!dbContext.ProductsTypes.Any())
                InitializeProductsTypes(dbContext);

            if (!dbContext.ServicesTypes.Any())
                InitializeServicesTypes(dbContext);

            if (!dbContext.ProducedProducts.Any() || !dbContext.HeatEnergyConsumptionRates.Any())
                InitializeProducedProductsAndHeatEnergyConsumptionRates(dbContext);

            if (!dbContext.ProvidedServices.Any())
                InitializeProvidedServices(dbContext);
        }

        static void InitializeOwnershipForms(HeatEnergyConsumptionContext dbContext)
        {
            string name;

            for (int i = 1; i <= 100; i++)
            {
                name = $"Форма собственности {i}";

                dbContext.OwnershipForms.Add(new OwnershipForm
                {
                    Name = name
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeManagers(HeatEnergyConsumptionContext dbContext)
        {
            string name;
            string surname;
            string middleName;
            string phoneNumber;

            for (int i = 1; i <= 100; i++) 
            {
                name = $"Имя {i}";
                surname = $"Фамилия {i}";
                middleName = $"Отчество {i}";
                phoneNumber = random.NextPhoneNumber();

                dbContext.Managers.Add(new Manager()
                {
                    Name = name,
                    Surname = surname,
                    MiddleName = middleName,
                    PhoneNumber = phoneNumber
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeOrganizations(HeatEnergyConsumptionContext dbContext)
        {
            string name;
            int ownershipFormId;
            string address;
            int managerId;

            for (int i = 1; i <= 500; i++)
            {
                name = $"Организация {i}";
                ownershipFormId = random.Next(1, dbContext.OwnershipForms.Count() + 1);
                address = $"Адрес {i}";
                managerId = random.Next(1, dbContext.Managers.Count() + 1);

                dbContext.Organizations.Add(new Organization()
                {
                    Name = name,
                    OwnershipFormId = ownershipFormId,
                    Address = address,
                    ManagerId = managerId
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeChiefPowerEngineers(HeatEnergyConsumptionContext dbContext)
        {
            string name;
            string surname;
            string middleName;
            string phoneNumber;
            int organizationId;

            for (int i = 1; i <= 500; i++)
            {
                name = $"Имя {i}";
                surname = $"Фамилия {i}";
                middleName = $"Отчество {i}";
                phoneNumber = random.NextPhoneNumber();
                organizationId = random.Next(1, dbContext.Organizations.Count() + 1);

                dbContext.ChiefPowerEngineers.Add(new ChiefPowerEngineer()
                {
                    Name = name,
                    Surname = surname,
                    MiddleName = middleName,
                    PhoneNumber = phoneNumber,
                    OrganizationId = organizationId
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeProductsTypes(HeatEnergyConsumptionContext dbContext)
        {
            string code;
            string name;
            string unit;

            for (int i = 1; i <= 100; i++)
            {
                code = random.NextCode(6);
                name = $"Тип продукции {i}";
                unit = $"ЕИ {i}";

                dbContext.ProductsTypes.Add(new ProductsType
                {
                    Code = code,
                    Name = name,
                    Unit = unit
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeServicesTypes(HeatEnergyConsumptionContext dbContext)
        {
            string code;
            string name;
            string unit;

            for (int i = 1; i <= 100; i++)
            {
                code = random.NextCode(6);
                name = $"Тип услуги {i}";
                unit = $"ЕИ {i}";

                dbContext.ServicesTypes.Add(new ServicesType
                {
                    Code = code,
                    Name = name,
                    Unit = unit
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeProducedProductsAndHeatEnergyConsumptionRates(HeatEnergyConsumptionContext dbContext)
        {
            int organizationId;
            int productTypeId;
            int productQuantity;
            int actualHeatEnergyQuantity;
            int normalHeatEnergyQuantity;
            DateTime date;

            for (int i = 1; i <= 500; i++)
            {
                organizationId = random.Next(1, dbContext.Organizations.Count() + 1);
                productTypeId = random.Next(1, dbContext.ProductsTypes.Count() + 1);
                productQuantity = random.Next(1, 1001);
                actualHeatEnergyQuantity = random.Next(1, 1001);
                normalHeatEnergyQuantity = random.Next(1, 1001);
                date = random.NextDate(2000, 2022);

                dbContext.ProducedProducts.Add(new ProducedProduct
                {
                    OrganizationId = organizationId,
                    ProductTypeId = productTypeId,
                    ProductQuantity = productQuantity,
                    HeatEnergyQuantity = actualHeatEnergyQuantity,
                    Date = date
                });

                dbContext.HeatEnergyConsumptionRates.Add(new HeatEnergyConsumptionRate
                {
                    OrganizationId = organizationId,
                    ProductTypeId = productTypeId,
                    Quantity = normalHeatEnergyQuantity,
                    Date = date
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeProvidedServices(HeatEnergyConsumptionContext dbContext)
        {
            int organizationId;
            int serviceTypeId;
            int quantity;
            DateTime date;

            for (int i = 1; i <= 500; i++)
            {
                organizationId = random.Next(1, dbContext.Organizations.Count() + 1);
                serviceTypeId = random.Next(1, dbContext.ServicesTypes.Count() + 1);
                quantity = random.Next(1, 101);
                date = random.NextDate(2000, 2022);

                dbContext.ProvidedServices.Add(new ProvidedService
                {
                    OrganizationId = organizationId,
                    ServiceTypeId = serviceTypeId,
                    Quantity = quantity,
                    Date = date
                });
            }

            dbContext.SaveChanges();
        }

        static DateTime NextDate(this Random random, int startYear, int endYear)
        {
            int day = 1;
            int month = random.Next(1, 13);
            int year = random.Next(startYear, endYear + 1);

            return new DateTime(year, month, day);
        }

        static string NextPhoneNumber(this Random random)
        {
            List<string> phoneCodes = new List<string>()
            {
                "25",
                "29",
                "33",
                "44"
            };

            string phoneNumber = "375" + phoneCodes[random.Next(phoneCodes.Count)];

            while (phoneNumber.Length != 12)
                phoneNumber += random.Next(10).ToString();

            return phoneNumber;
        }

        static string NextCode(this Random random, int length)
        {
            string symbols = "ABCDEFGHIJKLMNOPRSTUVWXYZ0123456789";
            string code = "";

            while (code.Length != length)
                code += symbols[random.Next(symbols.Length)];

            return code;
        }
    }
}