using herodesknew.Local.Application.SqlExecutionDocs.Commands.CreateSqlExecutionDoc;
using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Local.Server.Controllers;

[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public class SqlExecutionDocController : Controller
{
    private readonly CreateSqlExecutionDocHandler _createSqlExecutionDocHandler;

    public SqlExecutionDocController(CreateSqlExecutionDocHandler createSqlExecutionDocHandler)
    {
        _createSqlExecutionDocHandler = createSqlExecutionDocHandler;
    }

    [HttpPost("Create")]
    public IActionResult CreateSqlFile(int ticketId, int sqlFileId, string pullRequestUrl)
    {
        var result = _createSqlExecutionDocHandler.Handle(new(ticketId,sqlFileId,pullRequestUrl));

        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}
