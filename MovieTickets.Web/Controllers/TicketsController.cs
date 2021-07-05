using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DomainModels.Genre;
using MovieTickets.Domain.DTO;
using MovieTickets.Repository;
using MovieTickets.Repository.Interface;
using MovieTickets.Services.Interface;

namespace MovieTickets.Web.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        private readonly IMovieService _movieService;
        private static string _movieId;

        public TicketsController(ITicketService ticketService, IUserService userService, IMovieService movieService)
        {
            this._ticketService = ticketService;
            _userService = userService;
            _movieService = movieService;
        }


        // GET: Tickets
        [AllowAnonymous]
        public async Task<IActionResult> Index(int genre = -1)
        {

            if (genre >= 0)
            {
                GENRE g = (GENRE)Enum.GetValues(typeof(GENRE)).GetValue(genre);
                var applicationDbContext = _ticketService.GetAllTickets(g);
                return View(applicationDbContext);
            }
            else
            {
                var applicationDbContext = _ticketService.GetAllTickets();
                   // _context.Tickets.Include(t => t.Movie);
             return View(applicationDbContext);
            }

        }
      
        [AllowAnonymous]
        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create(Guid? MovieId)
        {
            _movieId = MovieId.ToString();
            ViewData["MovieName"] = _movieService.GetDetailsForMovie(MovieId).MovieName;
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create([Bind("TicketPrice,TicketTimeStamp,Id")] Ticket ticket)
        {
            if (ModelState.IsValid && ! String.IsNullOrEmpty(_movieId))
            {
                ticket.Id = Guid.NewGuid();
                Guid temp = Guid.Parse(_movieId);
                var movie = _movieService.GetDetailsForMovie(temp);
                ticket.Movie = movie;
                ticket.MovieName = movie.MovieName;
                ticket.Genre = movie.Genre;
                _movieId = String.Empty;
                _ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
          //  return NotFound();
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["MovieName"] = new SelectList(_movieService.GetAllMovies(), "MovieName", "MovieName");
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(Guid id, [Bind("MovieName,TicketPrice,TicketTimeStamp,Id")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }
            var movie = _movieService.GetDetailsForMovie(ticket.MovieName);
            ticket.Movie = movie;
            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieId"] = new SelectList(_movieService.GetAllMovies(), "Id", "Id", ticket.MovieId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.GetDetailsForTicket(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            _ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddTicket(Guid id)
        {
            var ticket = _ticketService.GetDetailsForTicket(id);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _userService.Get(userId);

            var dto = new AddToUserTicketsDto
            {
                TicketId = ticket.Id,
                SelectedProduct = ticket,
                Quantity = 1
            };

            var result = _ticketService.AddToUserTickets(dto, userId);

            var item = new TicketInUserTickets
            {
                Ticket = ticket,
                TicketId = ticket.Id,
                UserTickets = user.UserTickets,
                UserTicketsId = user.UserTickets.Id
            };

            _userService.Update(user);
                  
            return RedirectToAction("Index", "UserTickets");
        }

        [Authorize(Roles = "Administrator")]
        private bool TicketExists(Guid id)
        {
            return _ticketService.TicketExists(id);
        }

        [Authorize(Roles = "Administrator")]
        public FileContentResult ExportTickets(int genre = -1)
        {
            var tickets = genre == -1 ? _ticketService.GetAllTickets() : _ticketService.GetAllTickets((GENRE)Enum.GetValues(typeof(GENRE)).GetValue(genre));
            string fileName = "Tickets.xls";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add(genre == -1 ? "All Tickets" : "All " + Enum.GetName(typeof(GENRE), genre) + " Tickets" );
                worksheet.Cell(1, 1).Value = "Movie";
                worksheet.Cell(1, 2).Value = "Start Time";
                worksheet.Cell(1, 3).Value = "Price ($)";

                for (int i=1; i <= tickets.Count(); i++)
                {
                    var ticket = tickets[i - 1];
                    worksheet.Cell(i + 1, 1).Value = ticket.MovieName;
                    worksheet.Cell(i + 1, 2).Value = ticket.TicketTimeStamp;
                    worksheet.Cell(i + 1, 3).Value = ticket.TicketPrice;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
