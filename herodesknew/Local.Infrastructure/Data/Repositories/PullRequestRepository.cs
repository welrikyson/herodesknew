using DocumentFormat.OpenXml.Bibliography;
using herodesknew.Local.Domain.Entities;
using herodesknew.Local.Domain.Repositories;
using herodesknew.Local.Domain.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace herodesknew.Local.Infrastructure.Data.Repositories
{
    public sealed class PullRequestRepository : IPullRequestRepository
    {
        private readonly SqlExecutionPlanDoc _sqlExecutionPlanDoc;
        private readonly string _rootTicketFolderPath;
        private readonly string defaultScriptFolderName = "SLT";

        public PullRequestRepository(SqlExecutionPlanDoc sqlExecutionPlanDoc)
        {
            _rootTicketFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS", "src", "HD");            
            _sqlExecutionPlanDoc = sqlExecutionPlanDoc;
        }

        public IEnumerable<PullRequest> GetPullRequests(int ticketId)
        {            
            string[] tickets = new string[] { ticketId.ToString() };
            
            var ticketDir = new DirectoryInfo(_rootTicketFolderPath);
            var currentYear = DateTime.Now.Year;
            var dx = ticketDir.GetDirectories();
            var yearDirectories = ticketDir.GetDirectories().Where(dir => dir.IsNumber((v) => v <= currentYear));
            var directories = yearDirectories.SelectMany(d => d.GetDirectories().Where(dir => dir.IsNumber((v) => v >= 1 && v <= 12)));

            var result = directories
                .SelectMany(d => d.GetDirectories().Where(d => d.IsNumber()))
                .Where(dir => dir.IsNumber()).Where(d => tickets.Contains(d.Name));
            

            var ticketFolder = result.SingleOrDefault();
            if(ticketFolder == null) return Enumerable.Empty<PullRequest>();

            string[] ticketSqlSubFolders = Directory.GetDirectories(Path.Combine(ticketFolder.FullName, "SLT"));

            var sqlFiles =
                ticketSqlSubFolders.Select(Path.GetFileName)
                    .Where(name => int.TryParse(name, out _))
            .Select(name => int.Parse(name!))
                    .Where(id => File.Exists(Path.Combine(ticketFolder.FullName, defaultScriptFolderName, $"{id}", SqlExecutionPlanDoc.FileName)))
            .Select(id =>
            {
                return new PullRequest() 
                {
                    TicketId = ticketId,
                    Id = _sqlExecutionPlanDoc.GetPullRequestId(Path.Combine(ticketFolder.FullName, defaultScriptFolderName, $"{id}")) ?? 0
                };                
            });
            
            return sqlFiles;
        }
    }
    public static class DirectoryExtension
    {

        public static bool IsNumber(this DirectoryInfo directory, Func<int, bool>? validate = null)
        {
            if (int.TryParse(directory.Name, out var value))
            {
                if (validate?.Invoke(value) ?? true)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
