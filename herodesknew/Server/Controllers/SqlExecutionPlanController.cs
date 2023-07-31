using herodesknew.Application.SqlExecutionPlans.Commands.UseSqlExecutionPlan;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class SqlExecutionPlanController : Controller
    {
        private readonly SqlExecutionPlanCommandHandler _sqlExecutionPlanCommandHandler;

        public SqlExecutionPlanController(SqlExecutionPlanCommandHandler sqlExecutionPlanCommandHandler)
        {
            _sqlExecutionPlanCommandHandler = sqlExecutionPlanCommandHandler;
        }

        [HttpPost("Use")]
        public IActionResult UseSqlExecutionPlanDoc(int sqlFileId, int ticketId)
        {
            var result = _sqlExecutionPlanCommandHandler.Handle(new SqlExecutionPlanCommand() { SqlFileId = sqlFileId, TicketId = ticketId });
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            return Ok();
        }

    }
}
