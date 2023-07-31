namespace herodesknew.Application.PullRequests.Commands.CreatePullRequest
{
    public sealed class CreatePullRequestCommand
    {
        public required int TicketId { get; set; }
        public required int SqlFileId { get; set; }
    }
}
