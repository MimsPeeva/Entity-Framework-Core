using P02_FootballBetting.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        public User()
        {
            Bets = new HashSet<Bet>();
        }
        [Key]
        public int UserId { get; set; }

        [MaxLength(ValidationConstants.UsernameMaxLength)]
        [Required]
        public string Username { get; set; } = null!;

        [MaxLength(ValidationConstants.UserNameMaxLength)]

        public string Name { get; set; } = null!;

        [MaxLength(ValidationConstants.PasswordMaxLength)]
        public string Password { get; set; } = null!;
        [MaxLength(ValidationConstants.EmailMaxLength)]

        public string Email { get; set; } = null!;

        public decimal Balance { get; set; }
        public ICollection<Bet> Bets { get; set; }
    }
}
