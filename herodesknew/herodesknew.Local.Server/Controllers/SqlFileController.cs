using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile;
using herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile;
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
    private readonly GetSqlFileQueyHandler _getSqlFileQueyHandler;

    public SqlFileController(OpenSqlFileCommandHandler openSqlFileCommandHandler, CreateSqlFileCommandHandler createSqlFileCommandHandler, GetSqlFileQueyHandler getSqlFileQueyHandler)
    {
        _openSqlFileCommandHandler = openSqlFileCommandHandler;
        _createSqlFileCommandHandler = createSqlFileCommandHandler;
        _getSqlFileQueyHandler = getSqlFileQueyHandler;
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

    [HttpGet]
    public IActionResult GetSqlFilePath(int ticketId, int sqlFileId)
    {                
        var result = _getSqlFileQueyHandler.Handle(new GetSqlFileQuey(ticketId, sqlFileId));

        return result.IsSuccess ? Ok(new { SqlFilePath = result.Value }) : BadRequest(result.Error);
    }
}
