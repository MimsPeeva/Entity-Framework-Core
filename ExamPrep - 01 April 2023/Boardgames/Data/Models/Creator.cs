using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boardgames.Data.Models.DataConstraints;
namespace Boardgames.Data.Models
{
    public class Creator
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(CreatorFirstNameMaxValue)]
        public string FirstName { get; set; } 
        [Required]
        [MaxLength(CreatorLastNameMaxValue)]
        public string LastName { get; set; }

        public ICollection<Boardgame> Boardgames { get; set; } = new List<Boardgame>();

    }
}
