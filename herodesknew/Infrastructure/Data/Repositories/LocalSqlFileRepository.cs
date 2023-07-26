using DocumentFormat.OpenXml.Office2010.Excel;
using herodesknew.Application.Local;
using herodesknew.Domain.Repositories;
using herodesknew.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Data.Repositories
{
    public class LocalSqlFileRepository : ISqlFileRepository
    {
        
        private readonly string _rootTicketFolderPath;
        private readonly SqlExecutionPlanDoc _sqlExecutionPlaDoc;
        private readonly string defaultScriptFolderName = "SLT";
        public LocalSqlFileRepository(SqlExecutionPlanDoc sqlExecutionPlaDoc)
        {
            _rootTicketFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS", "src", "HD");
            _sqlExecutionPlaDoc = sqlExecutionPlaDoc;
        }
        public IEnumerable<(string sqlFileName,int? pullRequestId)> GetSqlFiles(int ticketId)
        {
            var ticketFolder = FolderSearcher.FindFolderInDirectory(ticketId.ToString(),_rootTicketFolderPath);
            if (string.IsNullOrEmpty(ticketFolder))
            {
                return Enumerable.Empty<(string,int?)>();
            }

            string[] ticketSqlSubFolders = Directory.GetDirectories(Path.Combine(ticketFolder,"SLT"));
            
            var sqlFiles =  
                ticketSqlSubFolders.Select(Path.GetFileName)
                    .Where(name => int.TryParse(name, out _))
                    .Select(name => int.Parse(name!))
                    .Where(id => File.Exists(Path.Combine(ticketFolder, defaultScriptFolderName, $"{id}", $"{ticketId}--{id}.sql")))
                    .Select(id => (
                        sqlFileName: Path.Combine(ticketFolder, defaultScriptFolderName, $"{id}", $"{ticketId}--{id}.sql")
                        ,pullRequestId : _sqlExecutionPlaDoc.GetPullRequestId(Path.Combine(ticketFolder, defaultScriptFolderName, $"{id}"))));

            return sqlFiles;
        }
    }
}
