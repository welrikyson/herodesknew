using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;

namespace herodesknew.Local.Application.Tickets.Queries.GetTickets
{
    public sealed class GetTicketsQueryHandler
    {
        public Result<IEnumerable<GetTicketsQueryResponse>> Handle(GetTicketsQuery getTicketsQuery)
        {
            var ticketSubDirectoriesWithScripts = TicketFolderManager.GetTicketSubDirectoriesWithScripts(getTicketsQuery.Ids);
            var responses = ticketSubDirectoriesWithScripts
                .SelectMany(d => d)
                .Select(ticketDirectotyWithScript => new GetTicketsQueryResponse(
                    ticketDirectotyWithScript.TicketId,
                    ticketDirectotyWithScript.Id,
                    SqlExecutionPlanDoc.GetPullRequestId(ticketDirectotyWithScript.PathFullName)
                ));

            return Result.Success(responses);
        }
    }
}
