﻿using localshop.Core.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.Models
{
    // For update cart via Json
    public class Line
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }

    public class CartLine
    {
        public ProductDTO Product { get; set; }
        public int Quantity { get; set; }
    }

    public class Cart
    {
        public List<CartLine> LineCollection = new List<CartLine>();

        public decimal Summary
        {
            get { return LineCollection.Sum(l => (l.Product.DiscountPrice ?? l.Product.Price) * l.Quantity); }
        }

        public int SummaryQuantity
        {
            get { return LineCollection.Sum(l => l.Quantity); }
        }
    }
}