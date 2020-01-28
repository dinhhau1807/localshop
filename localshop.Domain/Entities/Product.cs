﻿using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Entities
{
    public class Product
    {
        public Product()
        {
            Price = 0;
            Quantity = 0;
            IsActive = true;
        }

        public string Id { get; set; }

        // Unique
        public string Sku { get; set; }

        // Unique
        public string MetaTitle { get; set; }

        public string Name { get; set; }

        public string ShortDesciption { get; set; }

        public string LongDescription { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateModified { get; set; }

        // Product Specifications (Detail)


        // FK_Status
        // Product status (In stock, Out of stock, ...)
        public string StatusId { get; set; }
        public Status Status { get; set;}

        // Product images
        public ICollection<Image> Images { get; set; }


        // FK_CategoryTable
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        // FK_Brand

        // Backlog, Backorder,...
    }
}