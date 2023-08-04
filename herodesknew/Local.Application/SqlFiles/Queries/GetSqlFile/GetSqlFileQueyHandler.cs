using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;

namespace herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile
{
    public sealed class GetSqlFileQueyHandler
    {
        public Result<string> Handle(GetSqlFileQuey getSqlFileQuey)
        {
            try
            {
                var (Id, dirSltFullName, PathFullName, TicketId) = TicketFolderManager.GetTicketSubDirectoriesWithScripts(new[] { getSqlFileQuey.ticketId })
                        .SelectMany(item => item)
                        .Single(sqlFile => sqlFile.Id == getSqlFileQuey.sqlFileId);

                return Path.Combine(PathFullName, $"{TicketId}--{Id}.sql");
            }
            catch (ArgumentNullException)
            {
                return Result.Failure<string>(Error.NullValue);
            }
        }
    }
}
