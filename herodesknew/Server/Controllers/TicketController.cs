using herodesknew.Application.Tickets.Queries.GetTickets;
using herodesknew.Domain.Repositories;
using herodesknew.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor;

namespace herodesknew.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly GetMembersQueryHandler _getMembersQueryHandler;

        public TicketController(ILogger<TicketController> logger, GetMembersQueryHandler getMembersQueryHandler)
        {
            _logger = logger;
            _getMembersQueryHandler = getMembersQueryHandler;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var result = await _getMembersQueryHandler.Handle(new GetTicketsQuery());
            if (result.IsSuccess)
            {
                return Ok(result.Value.ticketResponses);
            }
            else
            {
                return NotFound();
            }
            
        }

        [HttpPost("GetFilteredTickets")]
        public async Task<IActionResult> GetFilteredTickets([FromBody] List<Filter>? filters, int skip = 0, int take = 10)
        {
            var result = await _getMembersQueryHandler.Handle(new GetTicketsQuery() { Filters = filters, Skip = skip,Take=take});
            if (result.IsSuccess)
            {
                return Ok(new { Tickets =  result.Value.ticketResponses , TotalCount=result.Value.totalCount});
            }
            else
            {
                return NotFound();
            }
        }
    }
}
