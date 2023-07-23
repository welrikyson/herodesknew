using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace herodesknew.Infrastructure.Data.Repositories
{
    public sealed class PullRequestRepository : IPullRequestRepository
    {
        private readonly IPullRequestFetcher pullRequestFetcher;

        public PullRequestRepository(IPullRequestFetcher pullRequestFetcher)
        {
            this.pullRequestFetcher = pullRequestFetcher;
        }

        public IEnumerable<PullRequest> GetSolutions()
        {
            return pullRequestFetcher.FetchPullRequests();
        }
    }

    public class PullRequestFetcher : IPullRequestFetcher
    {
        private readonly string _fileName;
        private readonly string _baseFolder;
        private readonly ISpreadsheetHelper _spreadsheetHelper;
        private readonly IUrlParser _urlParser;

        public PullRequestFetcher(IConfiguration configuration, ISpreadsheetHelper spreadsheetHelper, IUrlParser urlParser)
        {
            _fileName = configuration["Files:PlanoDeploy"] ?? throw new ArgumentException("Files:PlanoDeploy not found in configuration.");
            _baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MS\\src");
            _spreadsheetHelper = spreadsheetHelper;
            _urlParser = urlParser;
        }

        public IEnumerable<PullRequest> FetchPullRequests()
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
                int ticketId = GetTicketIdFromFolder(Path.GetDirectoryName(solutionFolder));
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

        private int GetTicketIdFromFolder(string ticketFolder)
        {
            if (int.TryParse(Path.GetFileName(ticketFolder), out int ticketId))
            {
                return ticketId;
            }

            // Return a default value or handle the error case based on your application's requirements.
            // For example, you could throw an exception here if the folder name doesn't contain a valid ticket ID.
            return -1; // Change this to an appropriate default value or throw an exception.
        }
    }

    public interface IPullRequestFetcher
    {
        IEnumerable<PullRequest> FetchPullRequests();
    }

    public interface ISpreadsheetHelper
    {
        bool FileExists(string filePath);
        string? GetCellValue(string filePath, string cellReference);
    }

    public interface IUrlParser
    {
        bool TryGetPullRequestNumberFromUrl(string url, out int pullRequestNumber);
    }

    public class SpreadsheetHelper : ISpreadsheetHelper
    {
        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public string? GetCellValue(string filePath, string cellReference)
        {
            using var document = SpreadsheetDocument.Open(filePath, true);
            var worksheetPart = document.WorkbookPart?.WorksheetParts.Last();
            var cells = worksheetPart?.Worksheet.Descendants<Cell>();
            var cell = cells?.FirstOrDefault(c => c.CellReference == cellReference);
            return cell?.CellValue?.Text;
        }
    }

    public class UrlParser : IUrlParser
    {
        public bool TryGetPullRequestNumberFromUrl(string url, out int pullRequestNumber)
        {
            const string pattern = @"/pullrequest/(\d+)";
            var match = System.Text.RegularExpressions.Regex.Match(url, pattern);

            if (!match.Success || !int.TryParse(match.Groups[1].Value, out pullRequestNumber))
            {
                pullRequestNumber = 0;
                return false;
            }

            return true;
        }
    }
}
