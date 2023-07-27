using herodesknew.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile
{
    public class GetSqlFileQueryHandler
    {
        private readonly string _rootTicketFolderPath;
        private readonly SqlExecutionPlanDoc _sqlExecutionPlaDoc;
        private readonly string defaultScriptFolderName = "SLT";
        public GetSqlFileQueryHandler(SqlExecutionPlanDoc sqlExecutionPlaDoc)
        {
            _rootTicketFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS", "src", "HD");
            _sqlExecutionPlaDoc = sqlExecutionPlaDoc;
        }

        IEnumerable<(string, int?)> Handle(int ticketId)
        {
            var ticketFolder = FolderSearcher.FindFolderInDirectory(ticketId.ToString(), _rootTicketFolderPath);
            if (string.IsNullOrEmpty(ticketFolder))
            {
                return Enumerable.Empty<(string, int?)>();
            }

            string[] ticketSqlSubFolders = Directory.GetDirectories(Path.Combine(ticketFolder, "SLT"));

            var sqlFiles =
                ticketSqlSubFolders.Select(Path.GetFileName)
                    .Where(name => int.TryParse(name, out _))
            .Select(name => int.Parse(name!))
                    .Where(id => File.Exists(Path.Combine(ticketFolder, defaultScriptFolderName, $"{id}", $"{ticketId}--{id}.sql")))
            .Select(id => (
                        sqlFileName: Path.Combine(ticketFolder, defaultScriptFolderName, $"{id}", $"{ticketId}--{id}.sql")
                        , pullRequestId: _sqlExecutionPlaDoc.GetPullRequestId(Path.Combine(ticketFolder, defaultScriptFolderName, $"{id}"))));

            return sqlFiles;
        }
    }
}
