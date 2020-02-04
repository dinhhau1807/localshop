﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace localshop.Core.DTO
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            Price = 0;
            Quantity = 0;
            IsActive = true;
        }

        public string Id { get; set; }

        [Display(Name = "SKU")]
        [Required]
        public string Sku { get; set; }

        public string MetaTitle { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowHtml]
        [Display(Name = "Short description")]
        public string ShortDesciption { get; set; }

        [AllowHtml]
        [Display(Name = "Long description")]
        public string LongDescription { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateModified { get; set; }

        public IList<string> Images { get; set; }

        [Display(Name = "Status")]
        public string StatusId { get; set; }

        [Display(Name = "Category")]
        public string CategoryId { get; set; }
    }
}