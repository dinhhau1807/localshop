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
        IList<OrderDTO> Orders { get; }

        OrderDTO FindById(string id);

        IList<OrderDTO> GetOrders(string userId);

        IList<OrderDetailDTO> GetOrderDetails(string id);

        string GetOrderStatus(string orderStatusId);

        string GetPaymentMethod(string paymentMethodId);

        void SetNullDeleteUser(string userId);

        void SetNullDeleteProduct(string productId);

        string AddPaymentMethod(OrderDTO orderDTO, string paymentMethod);

        string UpdateStatus(OrderDTO orderDTO, string statusName);
        
        string AddPaymentMethod(string orderId, string paymentMethod);

        string UpdateStatus(string orderId, string statusName);

        OrderDTO Save(OrderDTO order, IList<OrderDetailDTO> orderDetails);
    }
}
