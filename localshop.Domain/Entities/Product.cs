using MassTransit;
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
            Id = NewId.Next().ToString();
            Quantity = 0;
            IsActive = true;
            DateAdded = DateTime.Now;
        }

        public string Id { get; set; }

        // Unique
        public string Sku { get; set; }

        // Unique
        public string MetaTitle { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        public decimal Price { get; set; }

        public decimal? DiscountPrice { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? DateModified { get; set; }

        // Product Specifications

        // Product status (In stock, Out of stock, ...)

        // Join Image table
        //public ICollection<Image> Images { get; set; }


        // FK_CategoryTable
        //public int? CategoryId { get; set; }
        //public Category Category { get; set; }

        // FK_Brand

        // Backlog, Backorder,...
    }
}
