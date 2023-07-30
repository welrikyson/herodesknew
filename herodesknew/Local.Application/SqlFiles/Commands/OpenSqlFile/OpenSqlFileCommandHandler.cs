using herodesknew.Local.Application.Tickets.Queries.GetTickets;
using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile
{
    public sealed class OpenSqlFileCommandHandler
    {   
        public Result Handle(OpenSqlFileCommand openSqlFileCommand)
        {
            var sqlFile = TicketFolderManager
                .GetTicketSubDirectoriesWithScripts(new[] { openSqlFileCommand.TicketId })
                .SelectMany(item => item)
                .Where(sqlFile => sqlFile.Id == openSqlFileCommand.SqlFileId)
                .Select(sqlFile => $"{sqlFile.PathFullName}/{sqlFile.TicketId}--{sqlFile.Id}.sql")
                .SingleOrDefault();

            if (!File.Exists(sqlFile)) return Result.Failure(new Error("SqlFile.Open","File don´t Found"));            

            FileOpener.Open(sqlFile);

            return Result.Success();
        }
    }
}
