﻿using AutoMapper;
using localshop.Core.DTO;
using localshop.Domain.Abstractions;
using localshop.Domain.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localshop.Domain.Concretes
{
    public class OrderRepository : IOrderRepository
    {
        private IMapper _mapper;
        private ApplicationDbContext _context;

        public OrderRepository(IMapper mapper, ApplicationDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public OrderDTO FindById(string id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return null;
            }
            return _mapper.Map<Order, OrderDTO>(order);
        }

        public IList<OrderDetailDTO> GetOrderDetails(string id)
        {
            var orderDetails = _context.OrderDetails.Where(od => od.OrderId == id).AsEnumerable()
                                                    .Select(od => _mapper.Map<OrderDetail, OrderDetailDTO>(od)).ToList();
            return orderDetails;
        }

        public string GetOrderStatus(string orderStatusId)
        {
            var orderStatus = _context.OrderStatuses.FirstOrDefault(os => os.Id == orderStatusId);
            if (orderStatus == null)
            {
                return null;
            }

            return orderStatus.Name;
        }

        public string GetPaymentMethod(string paymentMethodId)
        {
            var paymentMethod = _context.PaymentMethods.FirstOrDefault(pm => pm.Id == paymentMethodId);
            if (paymentMethod == null)
            {
                return null;
            }

            return paymentMethod.Name;
        }

        public string AddPaymentMethod(OrderDTO orderDTO, string paymentMethod)
        {
            var method = _context.PaymentMethods.FirstOrDefault(pm => pm.Name == paymentMethod);
            if (paymentMethod == null)
            {
                return null;
            }

            orderDTO.PaymentMethodId = method.Id;
            return method.Id;
        }

        public string UpdateStatus(OrderDTO orderDTO, string statusName)
        {
            var status = _context.OrderStatuses.FirstOrDefault(os => os.Name == statusName);
            if (status == null)
            {
                return null;
            }

            orderDTO.OrderStatusId = status.Id;
            return status.Id;
        }

        public OrderDTO Save(OrderDTO orderDTO, IList<OrderDetailDTO> orderDetailDTOs)
        {
            orderDTO.Id = "#" + string.Join("", NewId.Next().ToString("D").ToUpperInvariant().Split('-'));
            orderDTO.OrderDate = DateTime.Now;
            var order = _mapper.Map<OrderDTO, Order>(orderDTO);

            var orderDetails = new List<OrderDetail>();
            foreach (var orderDetailDTO in orderDetailDTOs)
            {
                var orderDetail = _mapper.Map<OrderDetailDTO, OrderDetail>(orderDetailDTO);
                orderDetail.OrderId = order.Id;
                orderDetails.Add(orderDetail);
            }

            try
            {
                _context.Orders.Add(order);
                _context.OrderDetails.AddRange(orderDetails);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return null;
            }

            return orderDTO;
        }
    }
}
