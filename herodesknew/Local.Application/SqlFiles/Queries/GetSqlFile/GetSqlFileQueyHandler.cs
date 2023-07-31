using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile
{
    public sealed class GetSqlFileQueyHandler
    {
        public Result<string> Handle(GetSqlFileQuey getSqlFileQuey)
        {
            try
            {
                var sqlFile = TicketFolderManager.GetTicketSubDirectoriesWithScripts(new[] { getSqlFileQuey.ticketId })
                        .SelectMany(item => item)
                        .Single(sqlFile => sqlFile.Id == getSqlFileQuey.sqlFileId);

                return Path.Combine(sqlFile.PathFullName, $"{sqlFile.TicketId}--{sqlFile.Id}.sql");
            }
            catch (ArgumentNullException)
            {
                return Result.Failure<string>(Error.NullValue);
            }
        }
    }
}
