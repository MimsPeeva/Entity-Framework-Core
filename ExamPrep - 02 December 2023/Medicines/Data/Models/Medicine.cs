﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicines.Data.Models.Enums;
using static Medicines.Data.Models.DataConstricts;
namespace Medicines.Data.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(MedicineNameMaxLength)]
        public string Name { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public DateTime ProductionDate { get; set; }
        [Required]
        public DateTime ExpiryDate { get; set; }
        [Required]
        [MaxLength(MedicineProducerMaxLength)]
        public string Producer { get; set; }
        [Required]
        [ForeignKey(nameof(Pharmacy))] 
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new List<PatientMedicine>();
    }
}
