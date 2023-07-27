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
        private readonly string _fileName;
        private readonly string _baseFolder;
        private readonly ISpreadsheetHelper _spreadsheetHelper;
        private readonly IUrlParser _urlParser;

        public PullRequestRepository(ISpreadsheetHelper spreadsheetHelper, IUrlParser urlParser)
        {
            _fileName = "SIST 030 - 01 - Plano de Deploy.xlsx";
            _baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS\\src");
            _spreadsheetHelper = spreadsheetHelper;
            _urlParser = urlParser;
        }
        
        public IEnumerable<PullRequest> GetPullRequests()
        {
            return Directory.EnumerateDirectories(_baseFolder, "*", SearchOption.AllDirectories)
                .SelectMany(GetPullRequestsFromSolutionFolder)
                .ToList();
        }

        private IEnumerable<PullRequest> GetPullRequestsFromSolutionFolder(string solutionFolder)
        {
            string planoDeployPath = Path.Combine(solutionFolder, _fileName);
            string? pullRequestUrl = GetPullRequestUrlFromPath(planoDeployPath);

            if (pullRequestUrl != null && _urlParser.TryGetPullRequestNumberFromUrl(pullRequestUrl, out int pullRequestNumber))
            {
                int ticketId = GetTicketIdFromPath(planoDeployPath);
                yield return new PullRequest()
                {
                    TicketId = ticketId,
                    Id = pullRequestNumber
                };
            }
        }

        private string? GetPullRequestUrlFromPath(string planoDeployPath)
        {
            if (!_spreadsheetHelper.FileExists(planoDeployPath))
            {
                return null;
            }

            const string RefCellPullRequestUrl = "D44";
            return _spreadsheetHelper.GetCellValue(planoDeployPath, RefCellPullRequestUrl);
        }

        private int GetTicketIdFromPath(string? planoDeployPath)
        {
            if (planoDeployPath == null || !_spreadsheetHelper.FileExists(planoDeployPath))
            {
                return -1;
            }

            const string RefCellPullRequestDescription = "D11";
            var pullRequestDescription = _spreadsheetHelper.GetCellValue(planoDeployPath, RefCellPullRequestDescription);


            if (pullRequestDescription == null) return -1;

            Match match = Regex.Match(pullRequestDescription, @"(\d+) - (.+)");

            if (!match.Success)
            {
                return -1;
            }

            if (int.TryParse(match.Groups[1].Value, out int ticketId))
            {
                return ticketId;
            }
            return -1; // Change this to an appropriate default value or throw an exception.
        }
    }
}
