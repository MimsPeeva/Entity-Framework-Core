using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Game
    {
        public Game()
        {
            Bets = new HashSet<Bet>();
            PlayersStatistics = new HashSet<PlayerStatistic>();
        }
        [Key]
        public int GameId { get; set; }

        public int HomeTeamId { get; set; }
        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; } = null!;

        public int AwayTeamId { get; set; }
        [ForeignKey(nameof(AwayTeamId))]
        public Team AwayTeam { get; set; } = null!;


        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public double HomeTeamBetRate { get; set; }

        public double AwayTeamBetRate { get; set; }

        public double DrawBetRate { get; set; }

        public DateTime DateTime { get; set; }
        public string Result { get; set; }
        public ICollection<PlayerStatistic> PlayersStatistics { get; set; }
        public ICollection<Bet> Bets { get; set; }

    }
}
