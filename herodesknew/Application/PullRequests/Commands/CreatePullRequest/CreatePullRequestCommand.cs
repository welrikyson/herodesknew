namespace herodesknew.Application.PullRequests.Commands.CreatePullRequest
{
    public sealed class CreatePullRequestCommand
    {
        public required int TicketId { get; set; }
        public required string Content { get; set; }
    }
}
