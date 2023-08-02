using herodesknew.Application.PullRequests.Commands.CreatePullRequest;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAllOrigins")]
    public sealed class PullRequestController : Controller
    {
        private readonly CreatePullRequestCommandHandler _createPullRequestCommandHandler;

        public PullRequestController(CreatePullRequestCommandHandler createPullRequestCommandHandler)
        {
            _createPullRequestCommandHandler = createPullRequestCommandHandler;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePullRequestAsync(int ticketId, string content)
        {
            var result = await _createPullRequestCommandHandler.Handle(new CreatePullRequestCommand() { TicketId = ticketId, Content = content });
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { Message = $"Pull request {result.Value} criado com sucesso com base nos IDs do item e arquivo!" });
        }
    }
}
