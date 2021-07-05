using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTickets.Services.Interface;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Web.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;

        public OrdersController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = this._orderService.GetAllOrders(userId);
            return View(model);
        }

        public IActionResult OrdersAdmin()
        {
            var model = this._orderService.GetAllOrders();
            return View(model);
        }

        public FileContentResult GenerateFaktura(Guid Id)
        {

            var user = _userService.Get(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var order = this._orderService.GetDetailsForOrder(Id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "TicketOrder.docx");

            var document = DocumentModel.Load(templatePath);
            /*
             {{OrderId}}
             {{Username}}
             {{TicketList}}
             {{Price}}
             */
            var totalPrice = 0.0f;
            document.Content.Replace("{{OrderId}}", order.Id.ToString());
            StringBuilder sb = new StringBuilder();
            foreach (var item in order.TicketsInOrders)
            {
                totalPrice += item.Ticket.TicketPrice;
                sb.AppendLine($"{item.Ticket.MovieName} ${item.Ticket.TicketPrice} {item.Ticket.TicketTimeStamp.ToString()}");
            }
            document.Content.Replace("{{Username}}", user.Email);
            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{Price}}", totalPrice.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "OrderDetails.pdf");

        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = this._userService.Get(userId).UserTickets;

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var totalPrice = 0.0f;
            foreach (var ticket in order.TicketsInUserTickets)
            {
                totalPrice += ticket.Ticket.TicketPrice;
            }

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = (Convert.ToInt32(totalPrice) * 100),
                Description = "MovieTickets Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                var result = this.Order();

                if (result)
                {
                    
                    return RedirectToAction("Index", "Orders");
                }
                else
                {
                    return RedirectToAction("Index", "Orders");
                }
            }

            return RedirectToAction("Index", "Orders");
        }

        private Boolean Order()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._orderService.OrderNow(userId);

            return result;
        }
    }
}
