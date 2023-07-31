using herodesknew.Domain.Services;
using herodesknew.Local.Application.SqlExecutionDocs.Commands.UseSqlExecutionDoc;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Services
{
    public sealed class SqlExecutionDocService : ISqlExecutionDocService
    {
        private readonly UseSqlExecutionDocCommandHandler _useSqlExecutionDocCommandHandler;

        public SqlExecutionDocService(UseSqlExecutionDocCommandHandler useSqlExecutionDocCommandHandler)
        {
            _useSqlExecutionDocCommandHandler = useSqlExecutionDocCommandHandler;
        }

        public Result UseSqlExecutionDoc(int ticketId, int sqlFileId)
        {
            return _useSqlExecutionDocCommandHandler.Handle(new UseSqlExecutionDocCommand() { SqlFileId = sqlFileId , TicketId = ticketId });            
        }
    }
}
