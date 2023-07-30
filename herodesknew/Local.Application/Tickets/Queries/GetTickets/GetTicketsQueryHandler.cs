using herodesknew.Local.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.Tickets.Queries.GetTickets
{
    public class GetTicketsQueryHandler
    {
        void Handle()
        {
            var ids = new int[]
            {
                756801,
            };
            var ticketSubDirectoriesWithScripts = TicketFolderManager.GetTicketSubDirectoriesWithScripts(ids);
            ticketSubDirectoriesWithScripts
                .SelectMany(d => d)
                .Select(ticketDirectotyWithScript => new
                {
                    ticketDirectotyWithScript.TicketId,
                    ticketDirectotyWithScript.Id,
                    PullRequestId = SqlExecutionPlanDoc.GetPullRequestId(ticketDirectotyWithScript.PathFullName),
                });
        }
    }
}
