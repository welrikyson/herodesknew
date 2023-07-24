using System;
using herodesknew.Domain.Entities;

namespace herodesknew.Domain.Repositories
{
	public interface IPullRequestRepository
	{
        void AddPullRequests(List<PullRequest> pullRequests);
        IEnumerable<PullRequest> GetPullRequests();
    }
}

