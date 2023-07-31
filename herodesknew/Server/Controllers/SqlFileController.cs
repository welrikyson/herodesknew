using herodesknew.Application.SqlFiles.Commands.CreateSqlFile;
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
    private readonly CreateSqlFileCommandHandler _createSqlFileCommandHandler;

    public SqlFileController(OpenSqlFileCommandHandler openSqlFileCommandHandler, CreateSqlFileCommandHandler createSqlFileCommandHandler)
    {
        _openSqlFileCommandHandler = openSqlFileCommandHandler;
        _createSqlFileCommandHandler = createSqlFileCommandHandler;
    }

    [HttpPost("Open")]
    public IActionResult OpenSqlFile(int sqlFileId, int ticketId)
    {
        var result = _openSqlFileCommandHandler.Handle(new OpenSqlFileCommand() { SqlFileId = sqlFileId, TicketId = ticketId });
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPost("Create")]
    public IActionResult CreateSqlFile(int ticketId)
    {
        var result = _createSqlFileCommandHandler.Handle(new (ticketId));
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}
