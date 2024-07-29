using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class DataConstraints
    {
        //Footballer
        public const byte FootballerNameMinLength = 2;
        public const byte FootballerNameMaxLength = 40;

        //Coach
        public const byte CoachNameMinLength = 2;
        public const byte CoachNameMaxLength = 40;

        //Team
        public const byte TeamNameMinLength = 3;
        public const byte TeamNameMaxLength = 40;
        public const string TeamNameRegex = @"^[A-Za-z0-9\s\.\-]{3,}$";
        public const int TeamNationalityMinLength = 2;
        public const int TeamNationalityMaxLength = 40;

    }
}
