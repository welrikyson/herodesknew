using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.PullRequests.Queries.GetPullRequests
{
    public sealed class PullRequestResponse
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public PullRequestResponse()
        {
            
        }
    }
}
