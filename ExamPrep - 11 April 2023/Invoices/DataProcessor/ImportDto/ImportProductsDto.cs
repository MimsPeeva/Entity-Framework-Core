﻿using Invoices.Data.Models.Enums;
using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using static Invoices.Data.Models.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportProductsDto
    {
        [Required]
        [MinLength(ProductNameMinLength)]
        [MaxLength(ProductNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal),ProductPriceMinValue, ProductPriceMaxValue)]
        public decimal Price { get; set; }
        [Required]
        [Range(ProductCategoryTypeMinValue,ProductCategoryTypeMaxValue)]
        public CategoryType CategoryType { get; set; }
        [Required]
        public int[] Clients { get; set; } = null!;
    }
}
