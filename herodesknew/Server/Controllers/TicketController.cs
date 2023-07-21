using Azure.Core;
using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.Tickets.Queries.GetTickets;
using herodesknew.Domain.Repositories;
using herodesknew.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor;
using System.IO;
using System.Net.Mime;

namespace herodesknew.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly GetMembersQueryHandler _getMembersQueryHandler;
        private readonly int _idSupportAgent;

        public TicketController(ILogger<TicketController> logger,
                                GetMembersQueryHandler getMembersQueryHandler, IConfiguration configuration)
        {
            _logger = logger;
            _getMembersQueryHandler = getMembersQueryHandler;
            _idSupportAgent = configuration.GetValue<int>("HelpdeskSettings:IdAtuante");            

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            _logger.LogInformation("IdSupportAgent: {int}", _idSupportAgent);
            var result = await _getMembersQueryHandler.Handle(new GetTicketsQuery() { IdSupportAgent = _idSupportAgent });
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
            _logger.LogInformation("IdSupportAgent: {idSupportAgent}", _idSupportAgent);
            var result = await _getMembersQueryHandler.Handle(new GetTicketsQuery() { IdSupportAgent = _idSupportAgent, Filters = filters, Skip = skip,Take=take});
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
