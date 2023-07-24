using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Data.Migrators.PullRequest;

public sealed class PullRequestDataMigrator
{
    private readonly IPullRequestRepository _pullRequestRepository;
    private readonly IPullRequestFetcher _pullRequestFetcher;

    public PullRequestDataMigrator(IPullRequestRepository pullRequestRepository, IPullRequestFetcher pullRequestFetcher) 
    {
        _pullRequestRepository = pullRequestRepository;
        _pullRequestFetcher = pullRequestFetcher;
    }

    public void MigratePullRequestsFromExcelToDatabase()
    {

        try
        {
            List<Domain.Entities.PullRequest> pullRequests = _pullRequestFetcher.FetchPullRequests().ToList();
            MigrateToDatabase(pullRequests);
        }
        catch (Exception ex)
        {
            // Lide com qualquer exceção que possa ocorrer durante a migração.
            Console.WriteLine("Erro ao migrar os pull requests: " + ex.Message);
        }
    }
    

    private void MigrateToDatabase(List<Domain.Entities.PullRequest> pullRequests)
    {
        _pullRequestRepository.AddPullRequests(pullRequests);
    }
}
