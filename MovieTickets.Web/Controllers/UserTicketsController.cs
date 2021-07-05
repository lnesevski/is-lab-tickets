using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Services.Interface;

namespace MovieTickets.Web.Controllers
{
    [Authorize]
    public class UserTicketsController : Controller
    {
        //private readonly IUserTicketsService _userTicketsService;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public UserTicketsController(IUserService userService, ITicketService ticketService)
        {
           
            _userService = userService;
            _ticketService = ticketService;
        }

        // GET: UserTicketsController
        public ActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTickets = _userService.Get(userId).UserTickets;
            return View(userTickets);
        }


        public ActionResult DeleteTicketFromUsertickets(Guid TicketId)
        {
            var ticketToRemove = _ticketService.GetDetailsForTicket(TicketId);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(userId);

            var itemToRemove = user.UserTickets.TicketsInUserTickets.FirstOrDefault(t => t.TicketId == TicketId);

            user.UserTickets.TicketsInUserTickets.Remove(itemToRemove);

            _userService.Update(user);

            return RedirectToAction(nameof(Index));
        }
    }
}
