﻿using localshop.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Abstractions
{
    public interface IOrderRepository
    {
        OrderDTO FindById(string id);

        IList<OrderDTO> GetOrders(string userId);

        IList<OrderDetailDTO> GetOrderDetails(string id);

        string GetOrderStatus(string orderStatusId);

        string GetPaymentMethod(string paymentMethodId);

        string AddPaymentMethod(OrderDTO orderDTO, string paymentMethod);

        string UpdateStatus(OrderDTO orderDTO, string statusName);

        OrderDTO Save(OrderDTO order, IList<OrderDetailDTO> orderDetails);
    }
}