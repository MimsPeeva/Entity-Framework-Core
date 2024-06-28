using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        public Color()
        {
            PrimaryKitTeams = new HashSet<Team>();
            SecondaryKitTeams = new HashSet<Team>();
        }
        [Key]
        public int ColorId { get; set; }

        [MaxLength(ValidationConstants.ColorNameMaxLength)]
        public string Name { get; set; } = null!;


        public ICollection<Team> PrimaryKitTeams { get; set; }
        public ICollection<Team> SecondaryKitTeams { get; set; }

    }
}
