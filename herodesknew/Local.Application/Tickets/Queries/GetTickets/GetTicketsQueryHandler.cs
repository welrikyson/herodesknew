using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
