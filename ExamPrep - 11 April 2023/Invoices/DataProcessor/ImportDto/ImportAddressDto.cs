using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Invoices.Data.Models.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    [XmlType(nameof(Address))]

    public class ImportAddressDto
    {
        [XmlElement(nameof(StreetName))]
        [Required]
        [MinLength(AddressStreetNameMinValue)]
        [MaxLength(AddressStreetNameMaxValue)]
        public string StreetName { get; set; } = null!;
        [XmlElement(nameof(StreetNumber))]
        [Required]
        public int StreetNumber { get; set; }
        [XmlElement(nameof(PostCode))]
        [Required]
        public string PostCode { get; set; }

        [XmlElement(nameof(City))]
        [Required]
        [MinLength(AddressCityNameMinValue)]
        [MaxLength(AddressCityNameMaxValue)]
        public string City { get; set; } = null!;

        [XmlElement(nameof(Country))]
        [Required]
        [MinLength(AddressCountryNameMinValue)]
        [MaxLength(AddressCountryNameMaxValue)]
        public string Country { get; set; } = null!;
    }
}
