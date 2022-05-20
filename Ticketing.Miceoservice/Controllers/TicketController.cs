using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing.Miceoservice.Controllers
{
     [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private readonly IBus _bus;

        public TicketController(IBus bus)
        {
            _bus = bus;
        }
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket1)
        {
            if (ticket1 != null)
            {
                ticket1.BookedOn = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket1);
                return Ok();
            }
            return BadRequest();
        }
    }
}
