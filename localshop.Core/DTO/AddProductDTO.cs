﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace localshop.Core.DTO
{
    public class AddProductDTO
    {
        [Required]
        public string Sku { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowHtml]
        public string ShortDesciption { get; set; }

        [AllowHtml]
        public string LongDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public string StatusId { get; set; }

        public string CategoryId { get; set; }

        public string Images { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}