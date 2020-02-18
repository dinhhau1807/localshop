using localshop.Core.DTO;
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

        IList<OrderDetailDTO> GetOrderDetails(string id);

        OrderDTO Save(OrderDTO order, IList<OrderDetailDTO> orderDetails);
    }
}
