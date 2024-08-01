using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Medicines.Data.Models.DataConstricts;
namespace Medicines.Data.Models
{
    public class Pharmacy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(PharmacyNameMaxLength)]
        public string Name { get; set; }
        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        [RegularExpression(PharmacyPhoneNumberRegex)]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression(PharmacyBooleanRegex)]
        public bool IsNonStop { get; set; }

        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}
