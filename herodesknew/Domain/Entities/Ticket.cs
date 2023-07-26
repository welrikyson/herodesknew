namespace herodesknew.Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public required int IdDepartment { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string UserEmail { get; set; }
        public required int SlaUsed { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public required StatusEnum Status { get; set; }
        public IEnumerable<Attachment>? Attachments { get; set; }
        public IEnumerable<PullRequest>? PullRequests { get; set; }
        public IEnumerable<(string sqlFileName, int? pullRequestId)>? SqlFiles { get; set; }
    }
}
