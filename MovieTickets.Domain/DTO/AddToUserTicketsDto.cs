using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Domain.DTO
{
    public class AddToUserTicketsDto
    {
        public Ticket SelectedProduct { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
