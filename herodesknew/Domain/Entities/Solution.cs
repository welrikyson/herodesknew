using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Entities
{
    public sealed class Solution
    {
        public required int IdTicket { get; set; }
        public required int Id { get; set; }
        public required string? PullRequestUrl { get; set; }
    }
}
