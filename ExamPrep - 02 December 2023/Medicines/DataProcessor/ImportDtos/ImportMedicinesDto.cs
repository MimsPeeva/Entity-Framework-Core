using Medicines.Data.Models;
using Medicines.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Medicines.Data.Models.DataConstricts;
namespace Medicines.DataProcessor.ImportDtos
{
    [XmlType(nameof(Medicine))]
    public class ImportMedicinesDto
    {
        [XmlAttribute("category")]
        [Range(MedicineCategoryMinValue, MedicineCategoryMaxValue)]
        public int Category { get; set; }

        [XmlElement("Name")]
        [Required]
        [MinLength(MedicineNameMinLength)]
        [MaxLength(MedicineNameMaxLength)]
        public string Name { get; set; } = null!;

        [XmlElement("Price")]
        [Range(MedicinePriceMinValue, MedicinePriceMaxValue)]
        public double Price { get; set; }

        [XmlElement("ProductionDate")]
        [Required]
        public string ProductionDate { get; set; }

        [XmlElement("ExpiryDate")]
        [Required]
        public string ExpiryDate { get; set; } 

        [XmlElement("Producer")]
        [Required]
        [MinLength(MedicineProducerMinLength)]
        [MaxLength(MedicineProducerMaxLength)]
        public string Producer { get; set; } 

    }
}
