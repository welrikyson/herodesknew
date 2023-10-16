using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;

namespace herodesknew.Local.Application.SqlExecutionDocs.Commands.UseSqlExecutionDoc
{
    public sealed class UseSqlExecutionDocCommandHandler
    {
        public Result Handle(UseSqlExecutionDocCommand useSqlExecutionDocCommand)
        {
            var sqlFile = TicketFolderManager
                .GetTicketSubDirectoriesWithScripts(new[] { useSqlExecutionDocCommand.TicketId })
                .SelectMany(item => item)
                .Where(sqlFile => sqlFile.Id == useSqlExecutionDocCommand.SqlFileId)
                .Select(sqlFile => $"{sqlFile.PathFullName}\\{SqlExecutionPlanDoc.FileName}")
                .SingleOrDefault();

            if (!File.Exists(sqlFile))
                return Result.Failure(new Error("SqlFile.Open", "File don´t Found"));

            var targetPath = Path.Combine(SqlExecutionPlanDoc.PathDefaultToUse, SqlExecutionPlanDoc.FileName);

            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            File.Copy(sqlFile, targetPath);
            KeyboardSimulator.SimulateCtrlAlt9Shortcut();
            return Result.Success(sqlFile);
        }
    }   
}
