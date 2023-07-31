using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlExecutionDocs.Commands.UseSqlExecutionDoc
{
    public sealed class UseSqlExecutionDocCommand
    {
        public required int TicketId { get; set; }
        public required int SqlFileId { get; set; }
    }
}
