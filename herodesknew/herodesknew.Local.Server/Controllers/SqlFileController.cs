using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Local.Server.Controllers;

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
    public IActionResult OpenSqlFile(int ticketId, int sqlFileId)
    {
        var result = _openSqlFileCommandHandler.Handle(new OpenSqlFileCommand() { TicketId = ticketId, SqlFileId = sqlFileId });

        return result.IsSuccess ? Ok(): BadRequest(result.Error);
        
    }

    [HttpPost("Create")]
    public IActionResult CreateSqlFile(int ticketId)
    {
        var result = _createSqlFileCommandHandler.Handle(new (ticketId));

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
