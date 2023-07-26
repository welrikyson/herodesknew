using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Domain.Utils;
using herodesknew.Infrastructure.Data.Contexts;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace herodesknew.Infrastructure.Data.Repositories
{
    public sealed class PullRequestRepository : IPullRequestRepository
    {
        private readonly HerodesknewDbContext _herodesknewDbContext;

        public PullRequestRepository(HerodesknewDbContext herodesknewDbContext)
        {
            _herodesknewDbContext = herodesknewDbContext;
        }

        public void AddPullRequest(PullRequest pullRequest)
        {
            _herodesknewDbContext.PullRequests.Add(pullRequest);
            _herodesknewDbContext.SaveChanges();
        }
        public void AddPullRequests(List<PullRequest> pullRequests)
        {
            var existingPullRequestIds = _herodesknewDbContext.PullRequests.Select(t => t.Id).ToList();
            var newPullRequests = pullRequests.Where(t => t.Id > 0 && !existingPullRequestIds.Contains(t.Id));

            // Adiciona apenas os tickets que ainda não existem no banco de dados.
            _herodesknewDbContext.PullRequests.AddRange(newPullRequests);
            _herodesknewDbContext.SaveChanges();
        }

        public IEnumerable<PullRequest> GetPullRequests()
        {
            return _herodesknewDbContext.PullRequests.ToList();
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

    public interface IPullRequestFetcher
    {
        IEnumerable<PullRequest> FetchPullRequests();
    }
}
