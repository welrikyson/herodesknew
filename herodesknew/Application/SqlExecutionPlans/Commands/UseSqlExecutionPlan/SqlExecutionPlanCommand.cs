using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.SqlExecutionPlans.Commands.UseSqlExecutionPlan
{
    public sealed class SqlExecutionPlanCommand
    {
        public int TicketId { get; set; }
        public int SqlFileId { get; set; }
    }
}
