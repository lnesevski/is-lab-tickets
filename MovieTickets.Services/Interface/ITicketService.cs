using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DomainModels.Genre;
using MovieTickets.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace MovieTickets.Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        List<Ticket> GetAllTickets(GENRE g);
        Ticket GetDetailsForTicket(Guid? Id);
        Ticket GetDetailsForTicketFromMovie(Guid? MovieId);
        void CreateNewTicket(Ticket t);
        void UpdateExistingTicket(Ticket t);
        AddToUserTicketsDto GetUserTicketsInfo(Guid? Id);
        void DeleteTicket(Guid Id);
        bool AddToUserTickets(AddToUserTicketsDto item, string userId);
        bool TicketExists(Guid Id);
    }
}
