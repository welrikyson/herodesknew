using herodesknew.Domain.Services;
using herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Services
{
    public sealed class SqlFileService : ISqlFileService
    {
        private readonly OpenSqlFileCommandHandler _openSqlFileCommandHandler;

        public SqlFileService(OpenSqlFileCommandHandler openSqlFileCommandHandler)
        {
            _openSqlFileCommandHandler = openSqlFileCommandHandler;
        }
        public Result OpenFile(int ticketId, int sqlFileId)
        {
            return _openSqlFileCommandHandler.Handle(new OpenSqlFileCommand() { SqlFileId = sqlFileId, TicketId = ticketId});
            
        }
    }
}
