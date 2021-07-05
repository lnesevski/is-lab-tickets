using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.Identity;
using MovieTickets.Repository;
using MovieTickets.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTickets.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private DbSet<MoviesUser> users;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
            this.users = context.Set<MoviesUser>();
        }

        public MoviesUser Get(string Id)
        {
            return this.users.Include(u => u.UserTickets).
                Include(u => u.UserTickets.TicketsInUserTickets).Include("UserTickets.TicketsInUserTickets.Ticket").SingleOrDefault(u => u.Id == Id);
        }

        public List<MoviesUser> GetAll()
        {
            return this.users.ToList();
        }

        public void Update(MoviesUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _context.Update(user);
            _context.SaveChanges();
        }
    }
}
