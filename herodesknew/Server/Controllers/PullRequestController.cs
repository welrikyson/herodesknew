using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAllOrigins")]
    public sealed class PullRequestController : Controller
    {
        [HttpPost("Create")]
        public IActionResult CreatePullRequest(int ticketId, int sqlFileId)
        {
            return Ok(new { Message = "Pull request criado com sucesso com base nos IDs do item e arquivo!" });
        }
    }
}
