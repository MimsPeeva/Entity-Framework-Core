using System.Globalization;
using System.Text;
using Invoices.Data.Models;
using Invoices.Data.Models.Enums;
using Invoices.DataProcessor.ImportDto;
using Invoices.Utilities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using Invoices.Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper helper = new XmlHelper();
            const string xmlRoot = "Clients";

            ICollection<Client> clientsToImport = new List<Client>();

            ImportClientDto[] deserializedDtos =
                helper.Deserialize<ImportClientDto[]>(xmlString, xmlRoot);
            foreach (ImportClientDto clientDto in deserializedDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> addressesToImport = new List<Address>();
                foreach (ImportAddressDto clientDtoAddress in clientDto.Addresses)
                {
                    if (!IsValid(clientDtoAddress))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address address = new Address()
                    {
                        City = clientDtoAddress.City,
                        StreetName = clientDtoAddress.StreetName,
                        StreetNumber = clientDtoAddress.StreetNumber,
                        PostCode = clientDtoAddress.PostCode,
                        Country = clientDtoAddress.Country
                    };
                    addressesToImport.Add(address);
                }
                Client newClient = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = addressesToImport
                };
                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClients,clientDto.Name));
            }
            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();
            return sb.ToString();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            ICollection<Invoice> invoices = new List<Invoice>();
            ImportInvoiceDto[] deserializedInvoices = 
                JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString)!;
            foreach (ImportInvoiceDto dto in deserializedInvoices)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isIssueDateValid = DateTime.TryParse
                (dto.IssueDate, CultureInfo.InvariantCulture, DateTimeStyles.None,
                    out DateTime issueDate);

                bool isDueDateValid = DateTime.TryParse
                    (dto.DueDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dueDate);

                if (isDueDateValid = false || isIssueDateValid==false
                    || DateTime.Compare(dueDate,issueDate)<0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Clients.Any(cl=>cl.Id==dto.ClientId))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new Invoice()
                {
                    ClientId = dto.ClientId,
                    DueDate = dueDate,
                    IssueDate = issueDate,
                    Amount = dto.Amount,
                    CurrencyType = (CurrencyType)dto.CurrencyType,
                    Number = dto.Number
                };
                invoices.Add(invoice);
                sb.AppendLine(string.Format(SuccessfullyImportedInvoices, dto.Number));
            }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            StringBuilder sb = new StringBuilder();
            ICollection<Product> productsToImport = new List<Product>();

            ImportProductsDto[] deserialixzesProducts = 
                JsonConvert.DeserializeObject<ImportProductsDto[]>(jsonString)!;

            foreach (ImportProductsDto dto in deserialixzesProducts)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product newProduct = new Product()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    CategoryType = (CategoryType)dto.CategoryType
                };
                ICollection<ProductClient>productClientsToImport = new List<ProductClient>();
                foreach (int clientId in dto.Clients.Distinct())
                {
                    if (!context.Clients.Any(cl=>cl.Id==clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    ProductClient productClient = new ProductClient()
                    {
                        ClientId = clientId,
                        Product = newProduct
                    };
                    productClientsToImport.Add(productClient);
                }

                newProduct.ProductsClients = productClientsToImport;
                productsToImport.Add(newProduct);
                sb.AppendLine(string.Format
                    (SuccessfullyImportedProducts, dto.Name, productClientsToImport.Count));
            }
            context.Products.AddRange(productsToImport);
            context.SaveChanges();
            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
