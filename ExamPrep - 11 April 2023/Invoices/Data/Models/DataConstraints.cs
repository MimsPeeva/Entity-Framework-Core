using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoices.Data.Models.Enums;
namespace Invoices.Data.Models
{
    public static class DataConstraints
    {
        //Product
        public const byte ProductNameMinLength = 9;
        public const byte ProductNameMaxLength = 30;
        public const string ProductPriceMinValue = "5.00";
        public const string ProductPriceMaxValue = "1000.00";

        public const int ProductCategoryTypeMinValue = (int)CategoryType.ADR;
        public const int ProductCategoryTypeMaxValue = (int)CategoryType.Tyres;

        //Address
        public const byte AddressStreetNameMinValue = 10;
        public const byte AddressStreetNameMaxValue = 20;
        public const byte AddressCityNameMinValue = 5;
        public const byte AddressCityNameMaxValue = 15;
        public const byte AddressCountryNameMinValue = 5;
        public const byte AddressCountryNameMaxValue = 15;

        //Invoice
        public const int InvoiceNumberMinValue = 1_000_000_000;
        public const int InvoiceNumberMaxValue = 1_500_000_000;

        public const byte InvoiceCurrencyTypeMinValue = (int)CurrencyType.BGN;
        public const byte InvoiceCurrencyTypeMaxValue = (int)CurrencyType.USD;
        //Client
        public const byte ClientNameMinLength = 10;
        public const byte ClientNameMaxLength = 25;
        public const byte ClientNumberVatMinLength = 10;
        public const byte ClientNumberVatMaxLength = 15;
    }
}
