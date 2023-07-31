using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.Application.Tickets.Queries.GetTicket;
using herodesknew.Local.Application.Tickets.Queries.GetTickets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public class TicketController : Controller
{
    private readonly GetFilteredTicketsQueryHandler _getFilteredTicketsQueryHandler;
    private readonly GetTicketQueryHandler _getTicketQueryHandler;

    public TicketController(GetFilteredTicketsQueryHandler getFilteredTicketsQueryHandler, GetTicketQueryHandler getTicketQueryHandler)
    {
        _getFilteredTicketsQueryHandler = getFilteredTicketsQueryHandler;
        _getTicketQueryHandler = getTicketQueryHandler;
    }

    [HttpPost("Filter")]
    public async Task<IActionResult> FilterTicketsAsync(int skip, int take)
    {
        var reponse = await _getFilteredTicketsQueryHandler.Handle(new GetFilteredTicketsQuery()
        {
            IdSupportAgent = 11981,
            Skip = skip,
            Take = take
        });

        return Ok(new
        {
            TotalCount = reponse.Value.totalCount,
            Tickets = reponse.Value.ticketResponses
        });
    }

    [HttpGet("{ticketId}")]
    public async Task<IActionResult> GetTicketByIdAsync(int ticketId)
    {
        var reponse = await _getTicketQueryHandler.Handle(new() { TicketId = ticketId });

        return Ok(reponse.Value);
    }
}
