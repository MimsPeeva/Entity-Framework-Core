using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Medicines.Data.Models;
using static Medicines.Data.Models.DataConstricts;

namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType("Pharmacy")]
    public class ImportPharmaciesDto
    {
        [Required]
        [XmlElement("Name")]
        [MaxLength(PharmacyNameMaxLength)]
        [MinLength(PharmacyNameMinLength)]
        public string Name { get; set; } 

        [Required]
        [XmlElement("PhoneNumber")]
        [RegularExpression(PharmacyPhoneNumberRegex)]
        public string PhoneNumber { get; set; } 

        [Required]
        [XmlAttribute("non-stop")]
        [RegularExpression(PharmacyBooleanRegex)]
        public string IsNonStop { get; set; } 

        [XmlArray("Medicines")]
        public ImportMedicinesDto[] Medicines { get; set; }
    }
}
