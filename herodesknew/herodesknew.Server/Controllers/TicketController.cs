using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : Controller
    {
        private readonly GetFilteredTicketsQueryHandler _getFilteredTicketsQueryHandler;

        public TicketController(GetFilteredTicketsQueryHandler getFilteredTicketsQueryHandler)
        {
            _getFilteredTicketsQueryHandler = getFilteredTicketsQueryHandler;
        }
        [HttpPost]
        public async Task<IActionResult> GetAsync(int skip, int take)
        {
            //(var tickets, var totalCount) = _helpdeskService.GetTickets(_idAtuanteHelpdesk, skip, take);
            var reponse = await _getFilteredTicketsQueryHandler.Handle(new GetFilteredTicketsQuery() { IdSupportAgent = 11981, Skip = skip, Take = take });
            //return Ok(new { tickets, totalCount });
            return Ok(new { TotalCount = reponse.Value.totalCount, Tickets = reponse.Value.ticketResponses });
        }
    }
}
