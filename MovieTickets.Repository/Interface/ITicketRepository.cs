using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DomainModels.Genre;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Repository.Interface
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAll();
        Ticket Get(Guid? Id);
        Ticket GetFromMovie(Guid? MovieId);
        void Insert(Ticket entity);
        void Update(Ticket entity);
        void Delete(Ticket entity);
        void Remove(Ticket entity);
        void SaveChanges();
        IEnumerable<Ticket> GetAll(GENRE g);
        bool ItemExists(Guid Id);
    }
}
