using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.PullRequests.Commands.CreatePullRequest
{
    public sealed class CreatePullRequestCommand
    {
        public required int TicketId { get; set; }
        public required string Content { get; set; }
    }
}
