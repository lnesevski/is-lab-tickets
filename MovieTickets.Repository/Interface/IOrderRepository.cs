using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        List<Order> GetAllOrders(string UserId);
        Order GetDetailsForOrder(BaseEntity model);
        Order GetDetailsForOrder(Guid Id);
        void Insert(Order order);

    }
}
