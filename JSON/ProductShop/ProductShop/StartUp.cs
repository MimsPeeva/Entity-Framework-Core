using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;
using System.Data;
using Newtonsoft.Json.Serialization;
using ProductShop.DTOs;
using ProductShop.DTOs.Export;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //01
            //string usersFile = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, usersFile));
            //02
            //string productsFile = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productsFile));
            //03
            //string categoriesFile = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesFile));
            //04
            //string catprodText = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, catprodText));
            //Console.WriteLine(GetProductsInRange(context));
            //Console.WriteLine(GetSoldProducts(context));
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            //Console.WriteLine(GetUsersWithProducts(context));

        }
        //01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Count}";
        }
        //02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count}";
        }
        //03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);
            categories.RemoveAll(c => c.Name == null);
            context.Categories.AddRange(categories);
            context.SaveChanges();
            return $"Successfully imported {categories.Count}";
        }
        //04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert
                .DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoriesProducts.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        //05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p=>p.Price>=500&&p.Price<=1000)
                .OrderBy(p=>p.Price)
                .Select(p=>new ExportProductDTO()
                {
                    Name = p.Name,
                    Price = p.Price,
                   Seller  =$"{p.Seller.FirstName} {p.Seller.LastName}" 
                })
                .ToList();
            return FormattingSerializeObject(products);
        }
        //06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
                .Select(u => new SellerWithProductsDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SoldProducts = u.ProductsSold.Select(p => new SoldProductsDto()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName,
                    })
                })
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToList();

            return FormattingSerializeObject(soldProducts);
        }
        //07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts
                        .Average(cp => cp.Product.Price)
                        .ToString("f2"),
                    TotalRevenue = c.CategoriesProducts
                        .Sum(cp => cp.Product.Price)
                        .ToString("f2")

                })
                .OrderByDescending(x => x.ProductsCount)
                .ToList();

            return FormattingSerializeObject(categories);
        }
        //08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null && p.Price != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null && p.Price != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                        .ToArray()
                })
                .OrderByDescending(u => u.SoldProducts.Length)
                .ToArray();

            var output = new
            {
                UsersCount = users.Length,
                Users = users.Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Length,
                        Products = u.SoldProducts
                    }
                })
            };
            return FormattingSerializeObject(output);

        }
        
        

        private static string FormattingSerializeObject(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}