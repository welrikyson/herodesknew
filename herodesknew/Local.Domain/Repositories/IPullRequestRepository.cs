using herodesknew.Local.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Domain.Repositories
{
    public interface IPullRequestRepository
    {
        IEnumerable<PullRequest> GetPullRequests();
    }
}
