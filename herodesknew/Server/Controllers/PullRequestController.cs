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
        public async Task<IActionResult> CreatePullRequestAsync([FromBody] CreatePullRequest createPullRequest)
        {
            var result = await _createPullRequestCommandHandler.Handle(new () 
            { 
                TicketId = createPullRequest.TicketId, 
                Content = createPullRequest.Content 
            });

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok(new { PullRequestUrl = result.Value });
        }
    }

    public record CreatePullRequest(int TicketId, string Content);
}
