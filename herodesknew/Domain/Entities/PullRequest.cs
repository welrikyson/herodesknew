using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Entities
{
    public class PullRequest
    {        
        public required string Url { get; init; }
        public required int TicketId { get; init; }
    }
}
