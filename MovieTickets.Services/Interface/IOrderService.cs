using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Services.Interface
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> GetAllOrders(string UserId);
        Order GetDetailsForOrder(BaseEntity model);
        Order GetDetailsForOrder(Guid Id);
        bool OrderNow(string userId);
    }
}
