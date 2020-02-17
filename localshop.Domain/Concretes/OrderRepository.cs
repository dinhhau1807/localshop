using AutoMapper;
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

        public bool Save(OrderDTO orderDTO, IList<OrderDetailDTO> orderDetailDTOs)
        {
            var order = _mapper.Map<OrderDTO, Order>(orderDTO);
            order.Id = "#" + string.Join("", NewId.Next().ToString("D").ToUpperInvariant().Split('-'));
            order.OrderDate = DateTime.Now;
            order.ShipDate = null;

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
            catch (Exception e)
            {
                return false;
            }

            return true;
        }
    }
}
