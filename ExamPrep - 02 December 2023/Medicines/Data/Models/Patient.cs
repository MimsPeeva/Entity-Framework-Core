using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medicines.Data.Models.Enums;
using static Medicines.Data.Models.DataConstricts;
namespace Medicines.Data.Models
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(PatientFullNameMaxLength)]
        public string FullName { get; set; }
        [Required] public AgeGroup AgeGroup { get; set; }
        [Required] public Gender Gender { get; set; }
        public ICollection<PatientMedicine> PatientsMedicines { get; set; } = new List<PatientMedicine>();
    }
}
