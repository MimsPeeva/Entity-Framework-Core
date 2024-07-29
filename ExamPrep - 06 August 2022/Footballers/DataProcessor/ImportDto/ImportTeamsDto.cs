using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Footballers.Data.Models.DataConstraints;
namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamsDto
    {
        [Required]
        [MinLength(TeamNameMinLength)]
        [MaxLength(TeamNameMaxLength)]
        [RegularExpression(TeamNameRegex)]

        public string Name { get; set; }

        [Required]
        [MinLength(TeamNationalityMinLength)]
        [MaxLength(TeamNationalityMaxLength)]
        public string Nationality { get; set; }

        [Required]
        public int Trophies { get; set; }
        public int[] Footballers { get; set; }


    }
}
