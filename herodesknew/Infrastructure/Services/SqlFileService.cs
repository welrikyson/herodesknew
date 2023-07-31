using herodesknew.Domain.Services;
using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
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
        private readonly CreateSqlFileCommandHandler _createSqlFileCommandHandler;

        public SqlFileService(OpenSqlFileCommandHandler openSqlFileCommandHandler, CreateSqlFileCommandHandler createSqlFileCommandHandler)
        {
            _openSqlFileCommandHandler = openSqlFileCommandHandler;
            _createSqlFileCommandHandler = createSqlFileCommandHandler;
        }
        public Result OpenFile(int ticketId, int sqlFileId)
        {
            return _openSqlFileCommandHandler.Handle(new OpenSqlFileCommand() { SqlFileId = sqlFileId, TicketId = ticketId});            
        }

        public Result CreateSqlFile(int ticketId)
        {
            return _createSqlFileCommandHandler.Handle(new(ticketId));
        }
    }
}
