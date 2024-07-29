using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static Footballers.Data.Models.DataConstraints;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]

    public class ImportCoachesDto
    {
        [Required]
        [MinLength(CoachNameMinLength)]
        [MaxLength(CoachNameMaxLength)]
        [XmlElement("Name")]
        public string Name { get; set; }
        [Required]
        [XmlElement("Nationality")]
        public string Nationality { get; set; }
        [XmlArray("Footballers")]
        public ImportFootballersDto[] Footballers { get; set; }

    }
}
