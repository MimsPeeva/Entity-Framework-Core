using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Team
    {
        public Team()
        {
            Players = new HashSet<Player>();
            HomeGames = new HashSet<Game>();
            AwayGames = new HashSet<Game>();
        }
        [Key]
        public int TeamId { get; set; }

        [MaxLength(ValidationConstants.TeamNameMaxLength)]
        [Required]
        public string Name { get; set; } = null!;
        [MaxLength(ValidationConstants.URLMaxLength)]
        public string LogoUrl { get; set; }
        [MaxLength(ValidationConstants.InitialsMaxLength)]
        public string Initials { get; set; } = null!;

        public decimal Budget { get; set; }
        public int PrimaryKitColorId { get; set; }
        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color PrimaryKitColor { get; set; } = null!;

        public int SecondaryKitColorId { get; set; }
        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryKitColor { get; set; } = null!;


        public int TownId { get; set; }
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;


        public ICollection<Player> Players { get; set; }
        public virtual ICollection<Game> HomeGames { get; set; }

        public virtual ICollection<Game> AwayGames { get; set; }
    }

  
}
