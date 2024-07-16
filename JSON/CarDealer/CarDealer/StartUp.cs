using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            //01//09
            //string usingFile = File.ReadAllText("../../../Datasets/suppliers.json");
            //Console.WriteLine(ImportSuppliers(context, usingFile));
            //02//10
            //string usingFile = File.ReadAllText("../../../Datasets/parts.json");
            //Console.WriteLine(ImportParts(context, usingFile));
            //03//11
            //string usingFile = File.ReadAllText("../../../Datasets/cars.json");
            //Console.WriteLine(ImportCars(context, usingFile));
            //04//12
            //string usingFile = File.ReadAllText("../../../Datasets/customers.json");
            //Console.WriteLine(ImportCustomers(context, usingFile));
            //05//13
            string usingFile = File.ReadAllText("../../../Datasets/sales.json");
            Console.WriteLine(ImportSales(context, usingFile));
            //07//14
            //Console.WriteLine(GetOrderedCustomers(context));
            //08//15
            //Console.WriteLine(GetCarsFromMakeToyota(context));
            //09//16
            //Console.WriteLine(GetLocalSuppliers(context));
            //10//17
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //11//18
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            //12//19
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        //01//09
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);
            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();
            return $"Successfully imported {suppliers.Count}.";
        }
        //02//10
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var parts = JsonConvert.DeserializeObject<List<Part>>(inputJson);

            var validSupplierIds = context.Suppliers
                .Select(x => x.Id)
                .ToArray();
            var partsWithValidSupplierIds = parts
                .Where(p => validSupplierIds.Contains(p.SupplierId))
                .ToArray();

            context.Parts.AddRange(partsWithValidSupplierIds);
            context.SaveChanges();

            return $"Successfully imported {partsWithValidSupplierIds.Length}.";
        }
        //03//11
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var carDtos = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);
            var cars = new HashSet<Car>();
            var parts = new HashSet<PartCar>();
            foreach (var carDto in carDtos)
            {
                var newCar = new Car()
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TraveledDistance = carDto.TraveledDistance
                };
                cars.Add(newCar);
                foreach (var partId in carDto.PartsId.Distinct())
                {
                    parts.Add(new PartCar()
                    {
                        Car = newCar,
                        PartId = partId
                    });
                }
            }
            context.Cars.AddRange(cars);
            context.PartsCars.AddRange(parts);
            context.SaveChanges();
return $"Successfully imported {cars.Count}.";
;
        }
        //04//12
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);
            context.Customers.AddRange(customers);
            context.SaveChanges();
           return $"Successfully imported {customers.Count}.";
        }
        //05//13
        //public static string ImportSales(CarDealerContext context, string inputJson)
        //{
        //    var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);
        //    context.Sales.AddRange(sales);
        //    context.SaveChanges();
        //    return $"Successfully imported {sales.Count}.";
        //}
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            ImportSaleDto[] salesDtos = JsonConvert.DeserializeObject<ImportSaleDto[]>(inputJson);

            Sale[] sales = JsonConvert.DeserializeObject<Sale[]>(inputJson);

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}.";
        }
        //06//14
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                     c.IsYoungDriver
                })
                .ToList();
            return FormattingSerializeObject(customers);
        }
        //07//15
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var cars = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    c.Id,
                    c.Make,
                    Model = c.Model,
                    c.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToList();
            return FormattingSerializeObject(cars);
        }
        //08//16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliers = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    PartsCount = p.Parts.Count
                })
                .ToList();
            return FormattingSerializeObject(suppliers);
        }
        //10//17
         public static string GetCarsWithTheirListOfParts(CarDealerContext context)
         {
             var cars = context.Cars
                 .Select(c => new
                 {
                     car = new
                     {
                     c.Make,
                     c.Model,
                     c.TraveledDistance

                     },
                     parts = c.PartsCars
                         .Select(p => new
                         {
                             p.Part.Name,
                             Price = p.Part.Price.ToString("F2")
                         })
                 })
                 .ToList();

             return FormattingSerializeObject(cars);
         }
        //11//18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customers = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.SelectMany(s => s.Car.PartsCars)
                        .Sum(s => s.Part.Price)
                })
                .OrderByDescending(c => c.SpentMoney)
                .ThenByDescending(c => c.BoughtCars)
                .ToList();
            return FormattingSerializeObject(customers);
        }
        //12//19
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },
                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("F2"),
                    price = s.Car.PartsCars
                        .Sum(pc => pc.Part.Price)
                        .ToString("F2"),
                    priceWithDiscount = (s.Car.PartsCars
                            .Sum(pc => pc.Part.Price) * (1 - s.Discount / 100))
                        .ToString("F2")
                });

            return JsonConvert.SerializeObject(sales, Formatting.Indented);
        }
     
    private static string FormattingSerializeObject(object obj)
    {
        var settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            //ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };
        return JsonConvert.SerializeObject(obj, settings);
    }
    }
}