using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boardgames.Data.Models.DataConstraints;
using Boardgames.Data.Models;
using Boardgames.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Storage;

namespace Boardgames.Data.Models

{
    public class Boardgame
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(BoardGameNameMaxLength)]
        public string Name { get; set; } 
        [Required]
    public double Rating { get; set; }

        [Required]
        public int YearPublished { get; set; }
        [Required]
public CategoryType CategoryType { get; set; }
        [Required]
        public string Mechanics { get; set; } 
        [ForeignKey(nameof(Creator))]
        public int CreatorId{ get; set; }
        public Creator Creator { get; set; }
        public ICollection<BoardgameSeller> BoardgamesSellers { get; set; } = new List<BoardgameSeller>();
    }
}
