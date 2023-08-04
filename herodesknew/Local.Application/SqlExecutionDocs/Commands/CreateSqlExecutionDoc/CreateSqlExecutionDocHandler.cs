using herodesknew.Local.Application.SqlFiles.Commands.OpenSqlFile;
using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlExecutionDocs.Commands.CreateSqlExecutionDoc;

public sealed class CreateSqlExecutionDocHandler
{
    public Result Handle(CreateSqlExecutionPlanDocCommand createSqlExecutionPlanDocCommand)
    {
        var sqlFileInfo = TicketFolderManager
                .GetTicketSubDirectoriesWithScripts(new[] { createSqlExecutionPlanDocCommand.TicketId })
        .SelectMany(item => item)
                .Where(sqlFile => sqlFile.Id == createSqlExecutionPlanDocCommand.SqlFileId)
                .SingleOrDefault();

        if (SqlExecutionPlanDoc.GetPullRequestId(sqlFileInfo.PathFullName) != null)
        {
            return Result.Failure(
                new Error("PullRequest.Create", $"File {createSqlExecutionPlanDocCommand.SqlFileId} in Ticket {createSqlExecutionPlanDocCommand.TicketId} has pullrequest"));
        }

        var sqlFile = $"{sqlFileInfo.PathFullName}\\{SqlExecutionPlanDoc.FileName}";

        var pathFullName = Path.GetDirectoryName(sqlFile);

        if (!File.Exists(sqlFile)) return Result.Failure(new Error("PullRequest.Create", "File don´t Found"));

        var title = FileReader.ReadFirstLineFromFile(sqlFile)?.ExtractAlphanumeric();

        SqlExecutionPlanDoc.CreateDeployDocAsync(pathFullName!, title!, createSqlExecutionPlanDocCommand.PullRequestUrl);
        return Result.Success();
    }
}
