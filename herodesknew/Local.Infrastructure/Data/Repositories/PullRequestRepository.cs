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
        public PullRequestRepository()
        {
            
        }
        
        public IEnumerable<PullRequest> GetPullRequests(int ticketId)
        {
            string docPath =
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var ticketPath = Path.Combine(docPath, "MS", "src", "HD");

            var directory = new DirectoryInfo(ticketPath);
            directory.GetDirectories();
            Directory.EnumerateDirectories(ticketPath, $"{ticketId}", new EnumerationOptions()
            {
                MaxRecursionDepth = 2,
            });
            return null;
        }           
    }
}
