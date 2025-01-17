﻿using Boardgames.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Boardgames.Data.Models.DataConstraints;
namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellersDto
    {
        [Required]
        [MinLength(SellerNameMinValue)]
        [MaxLength(SellerNameMaxValue)]
        public string Name { get; set; }

        [Required]
        [MinLength(SellerAddressMinValue)]
        [MaxLength(SellerAddressMaxValue)]
        public string Address { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        [RegularExpression(@"www\.[a-zA-Z0-9-]+\.com")]
        public string Website { get; set; }
        [JsonProperty("Boardgames")]
        public int[] BoardgamesId { get; set; }
    }
}
