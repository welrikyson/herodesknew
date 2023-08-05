using herodesknew.Local.Application.PullRequests.Commands.CreatePullRequest;
using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Local.Server.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public class PullRequestController : Controller
{
    private readonly CreatePullRequestCommandHandler _createPullRequestCommandHandler;

    public PullRequestController(CreatePullRequestCommandHandler createPullRequestCommandHandler)
    {
        _createPullRequestCommandHandler = createPullRequestCommandHandler;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreatePullRequestAsync(int ticketId, int sqlFileId)
    {
        var result = await _createPullRequestCommandHandler.Handle(new(ticketId, sqlFileId));

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
