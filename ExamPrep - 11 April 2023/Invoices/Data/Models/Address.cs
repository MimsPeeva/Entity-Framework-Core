using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Invoices.Data.Models.DataConstraints;
namespace Invoices.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(AddressStreetNameMaxValue)]
        public string StreetName { get; set; }
        [Required]
        public int StreetNumber { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        [MaxLength(AddressCityNameMaxValue)]
        public string City { get; set; }
        [Required]
        [MaxLength(AddressCountryNameMaxValue)]
        public string Country { get; set; }
      [Required]
        [ForeignKey(nameof(Client))]
      public int ClientId { get; set; }
      [Required]
        public Client Client { get; set; }
    }
}







//    · Client – Client