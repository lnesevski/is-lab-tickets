using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository;
using MovieTickets.Repository.Interface;
using MovieTickets.Services.Interface;

namespace MovieTickets.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IRepository<EmailMessage> mailRepository)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mailRepository = mailRepository;
        }

        public List<Order> GetAllOrders()
        {
            return this._orderRepository.GetAllOrders();
        }

        public List<Order> GetAllOrders(string UserId)
        {
            return this._orderRepository.GetAllOrders(UserId);
        }

        public Order GetDetailsForOrder(BaseEntity model)
        {
            return this._orderRepository.GetDetailsForOrder(model);
        }

        public Order GetDetailsForOrder(Guid Id)
        {
            return this._orderRepository.GetDetailsForOrder(Id);
        }

        public bool OrderNow(string userId)
        {
            var user = this._userRepository.Get(userId);

            var userTickets = user.UserTickets;

            EmailMessage message = new EmailMessage();
            message.MailTo = user.Email;
            message.Subject = "Successfully created order";
            message.Status = false;
            


            if (user != null && userTickets != null)
            {
                Order newOrder = new Order()
                {
                    User = user,
                    UserId = user.Id
                };


                List<TicketInOrder> ticketInUserTickets = userTickets.TicketsInUserTickets.Select(z => new TicketInOrder { 
                    Ticket = z.Ticket,
                    TicketId = z.TicketId,
                    Order = newOrder,
                    OrderId = newOrder.Id,
                    Quantity = z.Quantity
                }).ToList();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("Your order is completed. The order contains: ");

                var totalPrice = 0.0;

                for (int i=1; i < ticketInUserTickets.Count(); i++)
                {
                    var item = ticketInUserTickets[i - 1];
                    totalPrice += item.Ticket.TicketPrice;
                    sb.AppendLine(i + ". " + item.Ticket.Movie.MovieName + ", $" + item.Ticket.TicketPrice + ", quantity: " + item.Quantity); ;
                }

                sb.AppendLine($"Total price: ${totalPrice}");
                message.Content = sb.ToString();

                newOrder.TicketsInOrders = ticketInUserTickets;

                this._mailRepository.Insert(message);
                this._orderRepository.Insert(newOrder);


                return true;
            }

            return false;
        }
    }
}
