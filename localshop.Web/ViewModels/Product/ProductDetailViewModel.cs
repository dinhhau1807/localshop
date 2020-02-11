using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.ViewModels
{
    public class ProductDetailViewModel
    {
        public ProductDTO Product { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }


        public IList<ProductRelatedViewModel> Related { get; set; }
    }
}