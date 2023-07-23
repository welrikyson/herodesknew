using System;
using herodesknew.Domain.Entities;

namespace herodesknew.Domain.Repositories
{
	public interface IPullRequestRepository
	{
        IEnumerable<PullRequest> GetSolutions();
    }
}

