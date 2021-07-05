using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.Identity;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTickets.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<MoviesUser> users;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            this.users = context.Set<MoviesUser>();
        }

        public void Delete(MoviesUser entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("user");
            }
            this._context.Remove(entity);
            _context.SaveChanges();
        }

        public MoviesUser Get(string Id)
        {
            return this.users.Include(u => u.UserTickets).Include(u => u.UserTickets.TicketsInUserTickets).
                Include("UserTickets.TicketsInUserTickets.Ticket").
                Include("UserTickets.TicketsInUserTickets.Ticket.Movie").FirstOrDefault(u => u.Id == Id);
        }

        public IEnumerable<MoviesUser> GetAll()
        {
            return users.AsEnumerable();
        }

        public void Insert(MoviesUser entity)
        {
            if (!this.users.Contains(entity))
            {
                this._context.Add(entity);
                this._context.SaveChanges();
            }
        }

        public void Update(MoviesUser entity)
        {
            if (this.users.Contains(entity))
            {
                this._context.Update(entity);
                this._context.SaveChanges();
            }
        }
    }
}
