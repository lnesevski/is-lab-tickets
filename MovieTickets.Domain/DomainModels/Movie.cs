using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieTickets.Domain.DomainModels.Genre;

namespace MovieTickets.Domain.DomainModels
{
    public class Movie : BaseEntity
    {
        public string MovieName { get; set; }
        public string MovieDescription { get; set; }
        public int MovieRating { get; set; }
        public int MovieDuration { get; set; }
        public GENRE Genre { get; set; }
    }
}
