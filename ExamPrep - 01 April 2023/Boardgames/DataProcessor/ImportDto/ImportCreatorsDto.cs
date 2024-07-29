using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Boardgames.Data.Models;
using static Boardgames.Data.Models.DataConstraints;
namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType(nameof(Creator))]
    public class ImportCreatorsDto
    {
        [Required]
        [MinLength(CreatorFirstNameMinValue)]
        [MaxLength(CreatorFirstNameMaxValue)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(CreatorLastNameMinValue)]
        [MaxLength(CreatorLastNameMaxValue)]
        public string LastName { get; set; }
        [Required]
        [XmlArray(nameof(Boardgames))]
        public ImportBoardgameDto[] Boardgames { get; set; }
    }
}
