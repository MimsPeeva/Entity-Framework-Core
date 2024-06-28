using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {
       
        [Key]
        public int CountryId { get; set; }

        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;


        public ICollection<Town> Towns { get; set; }
    }
}
