using herodesknew.Local.Application.SqlExecutionDocs.Commands.UseSqlExecutionDoc;
using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Local.Server.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public class SqlExecutionDocController : Controller
{
    private readonly UseSqlExecutionDocCommandHandler _useSqlExecutionDocCommandHandler;

    public SqlExecutionDocController(UseSqlExecutionDocCommandHandler useSqlExecutionDocCommandHandler)
    {        
        _useSqlExecutionDocCommandHandler = useSqlExecutionDocCommandHandler;
    }    

    [HttpPost("Use")]
    public IActionResult UseSqlFile(int ticketId, int sqlFileId)
    {
        var result = _useSqlExecutionDocCommandHandler.Handle(new(ticketId, sqlFileId));

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
