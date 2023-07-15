using System;
using System.Net.Mail;

namespace herodesknew.Shared
{
	public class Ticket
    {
        public int Id { get; set; }
        public required int IdDepartment { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int SlaUsed { get; set; }
        public bool IsClosed { get; set; }
        public IEnumerable<Attachment>? Attachments { get; set; }
        public IEnumerable<TicketSolution>? Solutions { get; set; }
    }

    public class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public byte[] FileContent { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }
    }

    public class TicketSolution
    {
        public required int IdTicket { get; set; }
        public required int Id { get; set; }
        public required string? PullRequestUrl { get; set; }
    }
}

