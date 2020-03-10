﻿using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace localshop.ViewModels
{
    public class TrackingViewModel
    {
        public OrderDTO Order { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentMethod { get; set; }

        public IList<OrderDetailDTO> OrderDetails { get; set; }
    }
}