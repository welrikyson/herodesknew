using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile
{
    public sealed class OpenSqlFileCommand
    {
        public required int TicketId { get; set; }
        public required int SqlFileId { get; set; }
    }
}
