using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Models
{
    public class CartLine
    {
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }

        public void AddItem(ProductDTO product, int quantity)
        {
            CartLine line = lineCollection.Where(p => p.Product.Id == product.Id).FirstOrDefault();

            if (line == null)
            {
                line = new CartLine
                {
                    Product = product,
                    Quantity = quantity
                };
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(ProductDTO product)
        {
            lineCollection.RemoveAll(l => l.Product.Id == product.Id);
        }

        public decimal Summary()
        {
            return lineCollection.Sum(l => (l.Product.DiscountPrice ?? l.Product.Price) * l.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }
    }
}