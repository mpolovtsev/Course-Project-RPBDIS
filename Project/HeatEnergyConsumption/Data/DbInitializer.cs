using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.Data
{
    public static class DbInitializer
    {
        static Random random = new Random();
        static string symbols = "ABCDEFGHIJKLMNOPRSTUVWXYZ0123456789";

        static List<string> ownershipFormNames = new List<string>
        {
            "Иностранная",
            "Коммунальная",
            "Республиканская",
            "Собственность граждан",
            "Собственность негосударственных юридических лиц без государственного и иностранного участия",
            "Собственность негосударственных юридических лиц с государственным участием без иностранного участия",
            "Собственность негосударственных юридических лиц с иностранным участием без государственного участия",
            "Собственность негосударственных юридических лиц с государственным и иностранным участием"
        };

        static List<string> personNames = new List<string>()
        {
            "Андрей",
            "Иван",
            "Константин",
            "Максим",
            "Пётр",
            "Роман",
            "Фёдор",
            "Ярослав"
        };

        static List<string> personSurnames = new List<string>()
        {
            "Бобров",
            "Колпаков",
            "Лавров",
            "Лапшин",
            "Лебедев",
            "Михеев",
            "Сорокин",
            "Токарев"
        };

        static List<string> personMiddleNames = new List<string>()
        {
            "Александрович",
            "Андреевич",
            "Артемьевич",
            "Георгиевич",
            "Иванович",
            "Матвеевич",
            "Миронович",
            "Станиславович"
        };

        static List<string> phoneCodes = new List<string>()
        {
            "25",
            "29",
            "33",
            "44"
        };

        static List<string> organizationNames = new List<string>()
        {
            "Ассоль",
            "Джемма",
            "ИнтерПрогресс",
            "ЛабТех",
            "Лиониус",
            "РоялГен",
            "Тонлит",
            "Вависон"
        };

        static List<string> organizationAddresses = new List<string>()
        {
            "г. Брест, ул. Лепешинского, 12",
            "г. Брест, ул. Советская, 45",
            "г. Витебск, ул. Трудовая, 10",
            "г. Гомель, ул. Абрамова, 56",
            "г. Гродно, ул. Садовая, 81",
            "г. Минск, ул. Федюнинского, 34",
            "г. Могилев, ул. Полевая, 98",
            "г. Мозырь, ул. Красноармейская, 14"
        };

        static List<(string, string)> productsTypeNamesAndUnits = new List<(string, string)>()
        {
            ("Макароны", "г"),
            ("Мука", "кг"),
            ("Ковровое покрытие", "кв. м"),
            ("Грузовой контейнер", "куб. м"),
            ("Минеральная вода", "л"),
            ("Краска", "мл"),
            ("Стальные бруски", "т"),
            ("Автомобиль", "шт"),
        };

        static List<string> servicesTypeNames = new List<string>()
        {
            "Диагностика",
            "Доставка",
            "Консультация",
            "Конференция",
            "Курс",
            "Ремонт",
            "Составление договора",
            "Тренинг"
        };

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
            foreach (string name in ownershipFormNames)
                dbContext.OwnershipForms.Add(new OwnershipForm() 
                { 
                    Name = name 
                });

            dbContext.SaveChanges();
        }

        static void InitializeManagers(HeatEnergyConsumptionContext dbContext)
        {
            string phoneNumber;

            for (int i = 0; i < personNames.Count; i++)
            {
                phoneNumber = "375" + phoneCodes[random.Next(phoneCodes.Count)];

                while (phoneNumber.Length != 12)
                    phoneNumber += random.Next(10).ToString();

                dbContext.Managers.Add(new Manager()
                {
                    Name = personNames[random.Next(personNames.Count)],
                    Surname = personSurnames[random.Next(personSurnames.Count)],
                    MiddleName = personMiddleNames[random.Next(personMiddleNames.Count)],
                    PhoneNumber = phoneNumber
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeOrganizations(HeatEnergyConsumptionContext dbContext)
        {
            for (int i = 0; i < 100; i++)
            {
                dbContext.Organizations.Add(new Organization
                {
                    Name = organizationNames[random.Next(organizationNames.Count)],
                    OwnershipFormId = random.Next(1, dbContext.OwnershipForms.Count() + 1),
                    Address = organizationAddresses[random.Next(organizationAddresses.Count)],
                    ManagerId = random.Next(1, dbContext.Managers.Count() + 1)
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeChiefPowerEngineers(HeatEnergyConsumptionContext dbContext)
        {
            string phoneNumber;

            for (int i = 0; i < 100; i++)
            {
                phoneNumber = "375" + phoneCodes[random.Next(phoneCodes.Count)];

                while (phoneNumber.Length != 12)
                    phoneNumber += random.Next(10).ToString();

                dbContext.ChiefPowerEngineers.Add(new ChiefPowerEngineer()
                {
                    Name = personNames[random.Next(personNames.Count)],
                    Surname = personSurnames[random.Next(personSurnames.Count)],
                    MiddleName = personMiddleNames[random.Next(personMiddleNames.Count)],
                    PhoneNumber = phoneNumber,
                    OrganizationId = random.Next(1, dbContext.Organizations.Count() + 1)
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeProductsTypes(HeatEnergyConsumptionContext dbContext)
        {
            string code;

            foreach ((string, string) productType in productsTypeNamesAndUnits)
            {
                code = "";

                while (code.Length != 6)
                    code += symbols[random.Next(symbols.Length)];

                dbContext.ProductsTypes.Add(new ProductsType
                {
                    Code = code,
                    Name = productType.Item1,
                    Unit = productType.Item2
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeServicesTypes(HeatEnergyConsumptionContext dbContext)
        {
            string code;

            foreach (string name in servicesTypeNames)
            {
                code = "";

                while (code.Length != 6)
                    code += symbols[random.Next(symbols.Length)];

                dbContext.ServicesTypes.Add(new ServicesType
                {
                    Code = code,
                    Name = name,
                    Unit = "шт"
                });
            }

            dbContext.SaveChanges();
        }

        static void InitializeProducedProductsAndHeatEnergyConsumptionRates(HeatEnergyConsumptionContext dbContext)
        {
            int organizationId;
            int productTypeId;
            int actualHeatEnergyQuantity;
            int normalHeatEnergyQuantity;
            DateTime date;

            for (int i = 0; i < 100; i++)
            {
                organizationId = random.Next(1, dbContext.Organizations.Count() + 1);
                productTypeId = random.Next(1, dbContext.ProductsTypes.Count() + 1);
                actualHeatEnergyQuantity = random.Next(1, 1001);
                normalHeatEnergyQuantity = random.Next(1, 1001);
                date = random.NextDate();

                dbContext.ProducedProducts.Add(new ProducedProduct
                {
                    OrganizationId = organizationId,
                    ProductTypeId = productTypeId,
                    ProductQuantity = random.Next(1, 1001),
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
            for (int i = 0; i < 100; i++)
            {
                dbContext.ProvidedServices.Add(new ProvidedService
                {
                    OrganizationId = random.Next(1, dbContext.Organizations.Count() + 1),
                    ServiceTypeId = random.Next(1, dbContext.ServicesTypes.Count() + 1),
                    Quantity = random.Next(1, 101),
                    Date = random.NextDate()
                });
            }

            dbContext.SaveChanges();
        }

        static DateTime NextDate(this Random random)
        {
            int day = 1;
            int month = random.Next(1, 13);
            int year = random.Next(2000, 2023);

            return new DateTime(year, month, day);
        }
    }
}