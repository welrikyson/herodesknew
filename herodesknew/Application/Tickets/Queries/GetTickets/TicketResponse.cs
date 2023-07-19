using herodesknew.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.Tickets.Queries.GetTickets
{
    public sealed class TicketResponse
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int SlaUsed { get; set; }
        public bool IsClosed { get; set; }
        public IEnumerable<Attachment>? Attachments { get; set; }
        public IEnumerable<Solution>? Solutions { get; set; }
    }
}
