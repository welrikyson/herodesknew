using herodesknew.Domain.Entities;

namespace herodesknew.Domain.Repositories
{
    public interface IPullRequestRepository
    {
        void AddPullRequest(PullRequest pullRequest);
        void AddPullRequests(List<PullRequest> pullRequests);
        IEnumerable<PullRequest> GetPullRequests();
    }
}

