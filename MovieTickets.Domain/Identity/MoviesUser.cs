using Microsoft.AspNetCore.Identity;
using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTickets.Domain.Identity
{
    public class MoviesUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual UserTickets UserTickets { get; set; }
    }
}
