using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTickets.Domain.DomainModels
{
    public class UserTickets : BaseEntity
    {
        public string UserId { get; set; }
        public virtual ICollection<TicketInUserTickets> TicketsInUserTickets { get; set; }
    }
}
