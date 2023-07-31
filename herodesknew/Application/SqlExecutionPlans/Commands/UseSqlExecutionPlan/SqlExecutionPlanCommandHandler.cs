using herodesknew.Domain.Services;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.SqlExecutionPlans.Commands.UseSqlExecutionPlan
{
    public sealed class SqlExecutionPlanCommandHandler
    {
        private readonly ISqlExecutionDocService _sqlExecutionDocService;

        public SqlExecutionPlanCommandHandler(ISqlExecutionDocService sqlExecutionDocService)
        {
            _sqlExecutionDocService = sqlExecutionDocService;
        }
        public Result Handle(SqlExecutionPlanCommand sqlExecutionPlanCommand)
        {
            return _sqlExecutionDocService.UseSqlExecutionDoc(sqlExecutionPlanCommand.TicketId, sqlExecutionPlanCommand.SqlFileId);
        }
    }
}
