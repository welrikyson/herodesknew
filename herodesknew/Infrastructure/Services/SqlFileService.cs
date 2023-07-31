using herodesknew.Domain.Services;
using herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile;
using herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile;
using herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile;
using herodesknew.Local.Application.Tickets.Queries.GetTickets;
using herodesknew.Shared;

namespace herodesknew.Infrastructure.Services
{
    public sealed class SqlFileService : ISqlFileService
    {
        private readonly OpenSqlFileCommandHandler _openSqlFileCommandHandler;
        private readonly CreateSqlFileCommandHandler _createSqlFileCommandHandler;
        private readonly GetSqlFileQueyHandler _getSqlFileQueyHandler;

        public SqlFileService(OpenSqlFileCommandHandler openSqlFileCommandHandler,
                              CreateSqlFileCommandHandler createSqlFileCommandHandler,
                              GetSqlFileQueyHandler getSqlFileQueyHandler)
        {
            _openSqlFileCommandHandler = openSqlFileCommandHandler;
            _createSqlFileCommandHandler = createSqlFileCommandHandler;
            _getSqlFileQueyHandler = getSqlFileQueyHandler;
        }
        public Result OpenFile(int ticketId, int sqlFileId)
        {
            return _openSqlFileCommandHandler.Handle(new OpenSqlFileCommand() { SqlFileId = sqlFileId, TicketId = ticketId});            
        }

        public Result CreateSqlFile(int ticketId)
        {
            return _createSqlFileCommandHandler.Handle(new(ticketId));
        }

        public Result<string> GetSqlFile(int ticketId, int sqlFileId)
        {
            return _getSqlFileQueyHandler.Handle(new GetSqlFileQuey(ticketId, sqlFileId));
        }
    }
}
