using Gatherly.Domain.Shared;
using herodesknew.Domain.Erros;
using herodesknew.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.PullRequests.Queries.GetPullRequests
{
    public sealed class GetPullRequestsQueryHandler
    {
        private readonly IPullRequestRepository _pullRequestRepository;

        public GetPullRequestsQueryHandler(IPullRequestRepository pullRequestRepository)
        {
            _pullRequestRepository = pullRequestRepository;
        }

        public Result<List<PullRequestResponse>?> Handle(GetPullRequestQuery getPullRequestQuery)
        {
            var pullRequests = _pullRequestRepository.GetPullRequests();
            var pullRequestResponseList = pullRequests?.Select((pullRequest) =>new PullRequestResponse() { Id = pullRequest.Id, TicketId = pullRequest.TicketId}).ToList();

            return Result.Success(pullRequestResponseList); 
        }
    }
}
