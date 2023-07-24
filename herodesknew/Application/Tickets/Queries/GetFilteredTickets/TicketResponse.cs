using herodesknew.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.Tickets.Queries.GetTickets;

public sealed class TicketResponse
{
    public required int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string UserEmail { get; set; }
    public required int SlaUsed { get; set; }
    public required StatusEnum Status { get; set; }
    public required DateTime? StartDate { get; set; }
    public required DateTime? CloseDate { get; set; }
    public IEnumerable<Attachment>? Attachments { get; set; }
    public IEnumerable<PullRequest>? PullRequests { get; set; }
}
