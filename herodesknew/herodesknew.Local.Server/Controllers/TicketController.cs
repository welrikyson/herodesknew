using herodesknew.Local.Application.Tickets.Queries.GetTickets;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Local.Server.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public class TicketController : Controller
{
    private readonly GetTicketsQueryHandler _getTicketsQueryHandler;

    public TicketController(GetTicketsQueryHandler getTicketsQueryHandler )
    {
        _getTicketsQueryHandler = getTicketsQueryHandler;
    }

    [HttpPost]
    public IActionResult GetTicketsFiltred(int[] ids)
    {
        var result = _getTicketsQueryHandler.Handle(new GetTicketsQuery() { Ids = ids});

        return result.IsSuccess ? 
            Ok(result.Value) : 
            BadRequest(result.Error);
    }
}
