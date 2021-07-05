using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Domain.DTO
{
    public class UserTicketsDto
    {
        public List<TicketInUserTickets> TicketsInUserTickets { get; set; }
        public float TotalPrice { get; set; }
    }
}
