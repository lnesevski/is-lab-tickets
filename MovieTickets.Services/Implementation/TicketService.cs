using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Services.Interface;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MovieTickets.Domain.DomainModels.Genre;

namespace MovieTickets.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<TicketInUserTickets> _ticketsInUserTicketsRepository;

        public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository, IRepository<TicketInUserTickets> ticketsInUserTicketsRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketsInUserTicketsRepository = ticketsInUserTicketsRepository;
        }

        public bool AddToUserTickets(AddToUserTicketsDto item, string userId)
        {
            var user = this._userRepository.Get(userId);
            var userTickets = user.UserTickets;

            if(item.TicketId != null && userTickets != null)
            {
                var ticket = this.GetDetailsForTicket(item.TicketId);

                TicketInUserTickets itemToAdd = new TicketInUserTickets {
                    Ticket = ticket,
                    TicketId = ticket.Id,
                    UserTickets = userTickets,
                    UserTicketsId = userTickets.Id,
                    Quantity = item.Quantity
                };

                this._ticketsInUserTicketsRepository.Insert(itemToAdd);
                
                return true;
            }
            return false;
        }

        public void CreateNewTicket(Ticket t)
        {
            this._ticketRepository.Insert(t);
        }

        public void DeleteTicket(Guid Id)
        {
            var ticket = this.GetDetailsForTicket(Id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            return this._ticketRepository.GetAll().ToList();
        }

      
        public List<Ticket> GetAllTickets(GENRE g)
        {
            return this._ticketRepository.GetAll(g).ToList();
        }

        public Ticket GetDetailsForTicket(Guid? Id)
        {
            return this._ticketRepository.Get(Id);
        }

        public Ticket GetDetailsForTicketFromMovie(Guid? MovieId)
        {
            return this._ticketRepository.GetFromMovie(MovieId);
        }

        public AddToUserTicketsDto GetUserTicketsInfo(Guid? Id)
        {
            var ticket = GetDetailsForTicket(Id);
            AddToUserTicketsDto dto = new AddToUserTicketsDto
            {
                SelectedProduct = ticket,
                TicketId = ticket.Id,
                Quantity = 1
            };
            return dto;
        }

        public void UpdateExistingTicket(Ticket t)
        {
            _ticketRepository.Update(t);
        }

        List<Ticket> ITicketService.GetAllTickets(GENRE g)
        {
            return _ticketRepository.GetAll(g).ToList();
        }

        bool TicketExists(Guid Id)
        {
            return _ticketRepository.ItemExists(Id);
        }

        bool ITicketService.TicketExists(Guid Id)
        {
            return _ticketRepository.ItemExists(Id);
        }
    }
}
