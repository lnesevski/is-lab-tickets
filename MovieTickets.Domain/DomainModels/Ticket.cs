using MovieTickets.Domain.DomainModels.Genre;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTickets.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
       [ForeignKey("Movie")]
        public Guid MovieId { get; set; }
        public string MovieName { get; set; }
        public Movie Movie { get; set; }
        [Required]
        public float TicketPrice { get; set; }
        [Required]
        public DateTime TicketTimeStamp { get; set; }
        public GENRE Genre { get; set; }
        public ICollection<TicketInUserTickets> TicketsInUserTickets { get; set; }
        public ICollection<TicketInOrder> TicketsInOrders { get; set; }
    }
}
