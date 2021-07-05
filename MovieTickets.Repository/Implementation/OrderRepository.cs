using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTickets.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Order> orders;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            this.orders = context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return this.orders.Include(o => o.User).Include(o => o.TicketsInOrders)
                .Include("TicketsInOrders.Ticket")
                .ToList();
        }

        public List<Order> GetAllOrders(string UserId)
        {
            return this._context.Orders.Include(o => o.TicketsInOrders)
                 .Include("TicketsInOrders.Ticket")
                 .Where(z => z.UserId == UserId).ToList();
        }

        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this.orders.Include(o => o.TicketsInOrders)
                .Include("TicketsInOrders.Ticket")
                .FirstOrDefault(o => o.Id == model.Id);
        }

        public Order GetDetailsForOrder(Guid Id)
        {
            return this.orders.Include(o => o.TicketsInOrders)
                .Include("TicketsInOrders.Ticket")
                .Include(o => o.User).FirstOrDefault(o => o.Id == Id);
        }

        public void Insert(Order order)
        {
            if (order != null)
            {
                this._context.Add(order);
                _context.SaveChanges();
            }
        }
    }
}
