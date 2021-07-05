using MovieTickets.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public MoviesUser User { get; set; }
        public ICollection<TicketInOrder> TicketsInOrders { get; set; }
    }
}
