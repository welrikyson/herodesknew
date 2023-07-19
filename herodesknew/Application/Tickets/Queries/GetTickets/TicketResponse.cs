using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.Tickets.Queries.GetTickets
{
    public sealed record TicketResponse
    {
        public int Id { get; internal set; }
    }
}
