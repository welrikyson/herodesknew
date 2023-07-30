using herodesknew.Application.SqlFiles.Commands.OpenSqlFile;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers;
[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public class SqlFileController : Controller
{
    private readonly OpenSqlFileCommandHandler _openSqlFileCommandHandler;

    public SqlFileController(OpenSqlFileCommandHandler openSqlFileCommandHandler)
    {
        _openSqlFileCommandHandler = openSqlFileCommandHandler;
    }
    [HttpPost]
    public IActionResult OpenSqlFile(int sqlFileId, int ticketId)
    {
        var result = _openSqlFileCommandHandler.Handle(new OpenSqlFileCommand() { SqlFileId = sqlFileId, TicketId = ticketId });
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
