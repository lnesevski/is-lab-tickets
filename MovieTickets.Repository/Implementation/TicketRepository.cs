using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DomainModels.Genre;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTickets.Repository.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Ticket> entities;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
            this.entities = _context.Set<Ticket>();
        }

        public void Delete(Ticket entity)
        {
            this.Remove(entity);
        }

        public Ticket Get(Guid? Id)
        {
            return this.entities.Include(t => t.Movie).FirstOrDefault(t => t.Id == Id);
        }

        public IEnumerable<Ticket> GetAll()
        {
            return this.entities.Include(t => t.Movie).AsEnumerable();
        }

        public IEnumerable<Ticket> GetAll(GENRE g)
        {
            return this.entities.Include(t => t.Movie).Where(t => t.Movie.Genre == g);
        }

        public Ticket GetFromMovie(Guid? MovieId)
        {
        
            var ticket = this.entities.Include(t => t.Movie).FirstOrDefault(t => t.Movie.Id == MovieId);
            return ticket;
            //return this.entities.SingleOrDefault(t => t.Movie.Id == MovieId);
        }

        public void Insert(Ticket entity)
        {
            this._context.Add(entity);
            this._context.SaveChanges();
        }

        public bool ItemExists(Guid Id)
        {
            return this.entities.Any(t => t.Id == Id);
        }

        public void Remove(Ticket entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            this._context.Remove(entity);
            this._context.SaveChanges();
        }

        public void SaveChanges()
        {
            this._context.SaveChanges();
        }

        public void Update(Ticket entity)
        {
            this._context.Update(entity);
            this._context.SaveChanges();
        }
    }
}
