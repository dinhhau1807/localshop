using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Models
{
    public class PriceFilter
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}