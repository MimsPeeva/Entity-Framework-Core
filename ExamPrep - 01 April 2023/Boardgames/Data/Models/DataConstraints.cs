using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.Data.Models
{
    public class DataConstraints
    {
        //Boardgame
        public const byte BoardGameNameMinLength = 10;
        public const byte BoardGameNameMaxLength = 20;
        public const double BoardgameRatingMinValue = 1.00;
        public const double BoardgameRatingMaxValue = 10.00;
        public const int BoardGameYearMinLength = 2018;
        public const int BoardGameYearMaxLength = 2023;

        //Creator
        public const byte CreatorFirstNameMinValue = 2;
        public const byte CreatorFirstNameMaxValue = 7;
        public const byte CreatorLastNameMinValue = 2;
        public const byte CreatorLastNameMaxValue = 7;

        //Seller
        public const byte SellerNameMinValue = 5;
        public const byte SellerNameMaxValue = 20;
        public const byte SellerAddressMinValue = 2;
        public const byte SellerAddressMaxValue = 30;
    }
}
