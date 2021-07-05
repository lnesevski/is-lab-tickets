using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieTickets.Domain.DomainModels
{
    public class TicketInUserTickets : BaseEntity
    {
        
        public Guid TicketId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
        public Guid UserTicketsId { get; set; }
        [ForeignKey("UserTicketsId")]
        public UserTickets UserTickets { get; set; }
        public int Quantity { get; set; }

    }
}