using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Sku { get; set; }

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

        public IList<string> Images { get; set; }

        public string StatusId { get; set; }

        public string CategoryId { get; set; }
    }
}
