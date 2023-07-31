using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlFiles.Commands.CreateSqlFile
{
    public sealed class CreateSqlFileCommandHandler
    {
        public Result Handle(CreateSqlFileCommand createSqlFileCommand)
        {
            var sqlFiles = TicketFolderManager
                .GetTicketSubDirectoriesWithScripts(new[] { createSqlFileCommand.ticketId })
                .SelectMany(item => item);
            
            if(sqlFiles.Where(s => SqlExecutionPlanDoc.GetPullRequestId(s.PathFullName) == null).Any())
            {
                return Result.Failure(new Error("SqlFile.Create", $"This ticket {createSqlFileCommand.ticketId} has sql don't send yet." ));
            }

            var sqlDir = sqlFiles.FirstOrDefault().dirSltFullName;

            if(sqlDir == null)
            {
                var sqlFileId = 1;
                var target = Path.Combine(TicketFolderSettings.Root, @$"{DateTime.Now:yyyy\\MM}", createSqlFileCommand.ticketId.ToString(), TicketFolderSettings.ScriptFolderName, sqlFileId.ToString());
                Directory.CreateDirectory(target);

                File.WriteAllText(Path.Combine(target, $"{createSqlFileCommand.ticketId}--{sqlFileId}.sql"), $"""
                                   --HD {createSqlFileCommand.ticketId}
                                   USE Corporate1
                                   """);
            }
            else
            {
                var sqlFileId = sqlFiles.Count() + 1;
                var target = Path.Combine(sqlDir, sqlFileId.ToString());
                Directory.CreateDirectory(target);

                File.WriteAllText(Path.Combine(target,$"{createSqlFileCommand.ticketId}--{sqlFileId}.sql"), $"""
                                   --HD {createSqlFileCommand.ticketId}
                                   USE Corporate1
                                   """);
            }            
            
            return Result.Success();
        }        
    }
}
