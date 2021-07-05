using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieTickets.Domain.DomainModels
{
    public class TicketInOrder : BaseEntity
    {
        public Guid TicketId { get; set; }
        public Guid OrderId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public int Quantity { get; set; }
    }
}
