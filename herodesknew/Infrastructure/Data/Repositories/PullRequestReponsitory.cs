using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Data.Contexts;

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
}
