using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();
            //09
            //string suppliersXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, suppliersXml));
            //10
            //string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsXml));
            //11
            //string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, carsXml));
            //12
            //string customersXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersXml));
            //13
            //string salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, salesXml));
            //14
            //Console.WriteLine(GetCarsWithDistance(context));
            //15
            //Console.WriteLine(GetCarsFromMakeBmw(context));
            //16
            //Console.WriteLine(GetLocalSuppliers(context));
            //17
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //18
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            //19
            Console.WriteLine(GetSalesWithAppliedDiscount(context));


        }

        //9
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SupplierImportDto[]),
                new XmlRootAttribute("Suppliers"));
            SupplierImportDto[] importDtos;
            using (var reader = new StringReader(inputXml))
            {
                importDtos = (SupplierImportDto[])xmlSerializer.Deserialize(reader);
            }

            ;

            Supplier[] suppliers = importDtos
                .Select(dto => new Supplier()
                {
                    Name = dto.Name,
                    IsImporter = dto.IsImporter
                }).ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }

        //10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PartsImportDto[]),
                new XmlRootAttribute("Parts"));

            PartsImportDto[] partsImportDtos;
            using (StringReader inReader = new StringReader(inputXml))
            {
                partsImportDtos = (PartsImportDto[])xmlSerializer.Deserialize(inReader);
            }

            var supplierIds = context.Suppliers
                .Select(s => s.Id)
                .ToArray();

            var partsWithValidSuppliers = partsImportDtos
                .Where(p => supplierIds.Contains(p.SupplierId))
                .ToArray();

            Part[] parts = partsWithValidSuppliers
                .Select(dto => new Part()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId
                }).ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";

        }
        //11
        //public static string ImportCars(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(CarImportDto[]),
        //        new XmlRootAttribute("Cars"));

        //    CarImportDto[] carImportDtos;
        //    using (StringReader reader = new StringReader(inputXml))
        //    {
        //        carImportDtos = (CarImportDto[])xmlSerializer.Deserialize(reader);
        //    };

        //    List<Car> cars = new List<Car>();

        //    foreach (var dto in carImportDtos)
        //    {
        //        Car car = new Car()
        //        {
        //            Make = dto.Make,
        //            Model = dto.Model,
        //            TraveledDistance = dto.TraveledDistance
        //        };

        //        int[] carPartsId = dto.PartIds
        //            .Select(p => p.Id)
        //            .Distinct()
        //            .ToArray();

        //        var carParts = new List<PartCar>();

        //        foreach (var id in carPartsId)
        //        {
        //            carParts.Add(new PartCar()
        //            {
        //                Car = car,
        //                PartId = id
        //            });
        //        }

        //        car.PartsCars = carParts;
        //        cars.Add(car);
        //    }
        //    context.AddRange(cars);
        //    context.SaveChanges();

        //    return $"Successfully imported {cars.Count}";
        //}
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var xmlSerializer = new XmlSerializer(typeof(CarImportDto[]),
                new XmlRootAttribute("Cars"));

            CarImportDto[] carDtos;
            using (var reader = new StringReader(inputXml))
            {
                carDtos = (CarImportDto[])xmlSerializer.Deserialize(reader);
            }

            var partIds = context.Parts.Select(p => p.Id).ToList();

            var cars = new List<Car>();

            foreach (var dto in carDtos)
            {
                var uniquePartIds = dto.PartIds
                    .Select(p => p.Id)
                    .Distinct()
                    .Where(id => partIds.Contains(id))
                    .ToList();

                var car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance,
                    PartsCars = uniquePartIds.Select(id => new PartCar() { PartId = id }).ToList()
                };

                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomersImportDto[]),
                new XmlRootAttribute("Customers"));
            CustomersImportDto[] customersDtos;
            using (StringReader reader = new StringReader(inputXml))
            {
                customersDtos = (CustomersImportDto[])xmlSerializer.Deserialize(reader);
            }

            Customer[] customers = customersDtos
                .Select(dto => new Customer()
                {
                    Name = dto.Name,
                    BirthDate = dto.BirthDate,
                    IsYoungDriver = dto.IsYoungDriver
                })
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Length}";
        }
        //13
        //public static string ImportSales(CarDealerContext context, string inputXml)
        //{
        //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(SalesImportDto[]),
        //        new XmlRootAttribute("Sales"));

        //    using StringReader reader = new StringReader(inputXml);

        //    SalesImportDto[] saleImportDtos = (SalesImportDto[])xmlSerializer.Deserialize(reader);

        //    int[] carIds = context.Cars
        //        .Select(x => x.Id)
        //        .ToArray();

        //    var validSalesImport = saleImportDtos
        //        .Where(dto => carIds.Contains(dto.CarId))
        //        .ToArray();

        //    Sale[] sales = validSalesImport
        //        .Select(vs => new Sale()
        //        {
        //            CarId = vs.CarId,
        //            CustomerId = vs.CustomerId,
        //            Discount = vs.Discount
        //        })
        //        .ToArray();


        //    context.Sales.AddRange(sales);
        //    context.SaveChanges();

        //    return $"Successfully imported {sales.Length}";
        //}
        //13
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SalesImportDto[]),
                new XmlRootAttribute("Sales"));

            using StringReader reader = new StringReader(inputXml);

            SalesImportDto[] saleImportDtos = (SalesImportDto[])xmlSerializer.Deserialize(reader);

            int[] carIds = context.Cars
                .Select(c => c.Id)
                .ToArray();

            //var validSalesImport = saleImportDtos
            //    .Where(dto => carIds.Contains(dto.CarId))
            //    .ToArray();

            //Sale[] sales = validSalesImport
            //    .Select(vs => new Sale()
            //    {
            //        CarId = vs.CarId,
            //        CustomerId = vs.CustomerId,
            //        Discount = vs.Discount
            //    })
            //    .ToArray();
            List<Sale> sales = new List<Sale>();

            foreach (var dto in saleImportDtos)
            {
                if (!carIds.Contains(dto.CarId))
                {
                    continue;
                }

                var sale = new Sale()
                {
                    CarId = dto.CarId,
                    CustomerId = dto.CustomerId,
                    Discount = dto.Discount
                };

                sales.Add(sale);
            }


            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carWithDistance = context.Cars
                .Where(c => c.TraveledDistance > 2000000)
                .Select(c => new CarWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();
            return SerializeToXml(carWithDistance, "cars");
        }

        //15
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmws = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new CarsFromMakeBMWDto()
                {
                    Id = c.Id,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ToArray();

            return SerializeToXml(bmws, "cars", true);
        }

        //16
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new GetLocalSuppliersDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                }).ToArray();
            return SerializeToXml(localSuppliers, "suppliers");
        }

        //17
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .Select(c => new GetCarsWithTheirListOfPartsDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                    Parts = c.PartsCars
                        .OrderByDescending(p => p.Part.Price)
                        .Select(pc => new PartsForCarsDto()
                        {
                            Name = pc.Part.Name,
                            Price = pc.Part.Price,
                        }).ToArray()
                }).ToArray();

            return SerializeToXml(carsWithParts, "cars");
        }

        //18
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var temp = context.Customers
                .Where(c => c.Sales.Count > 0)
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SalesInfo = c.Sales.Select
                        (s => new
                        {
                            Prices = c.IsYoungDriver
                                ? s.Car.PartsCars.Sum(s => Math.Round((double)s.Part.Price * 0.95, 2))
                                : s.Car.PartsCars.Sum(s => (double)s.Part.Price)
                        })
                        .ToArray()
                })
                .ToList();

            var customers = temp
                .OrderByDescending(c => c.SalesInfo.Sum(s => s.Prices))
                .Select(c => new TotalSalesByCustomerDto()
                {
                    BoughtCars = c.BoughtCars,
                    FullName = c.FullName,
                    SpentMoney = c.SalesInfo.Sum(s => (decimal)s.Prices)
                })
                .ToArray();

            return SerializeToXml(customers, "customers");
        }
        //19

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales
                .Select(s => new SaleWithDiscount()
                {
                    Car = new CarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TraveledDistance = s.Car.TraveledDistance
                    },
                    Discount = Math.Round((double)s.Discount,2),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartsCars
                        .Sum(pc => pc.Part.Price),
                    PriceWithDiscount = Math.Round(
                        (double)(s.Car.PartsCars.Sum(p => p.Part.Price)
                                 * (1 - (s.Discount / 100))), 4)
                }).ToArray();

            return SerializeToXml(sales, "sales");
        }


        private static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));
            StringBuilder stringBuilder = new StringBuilder();

            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true
            };

            using (StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings))
            {
                XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
                xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                try
                {
                    xmlSerializer.Serialize(xmlWriter, dto, xmlSerializerNamespaces);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return stringBuilder.ToString();
        }
    }
}