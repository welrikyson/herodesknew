using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlExecutionDocs.Commands.CreateSqlExecutionDoc
{
    public record CreateSqlExecutionPlanDocCommand(int TicketId, int SqlFileId, string PullRequestUrl);
}
