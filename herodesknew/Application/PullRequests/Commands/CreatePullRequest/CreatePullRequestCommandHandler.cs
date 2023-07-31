using herodesknew.Domain.AppSettingEntities;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Domain.Services;
using herodesknew.Shared;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using NUlid;
using System.Text;

namespace herodesknew.Application.PullRequests.Commands.CreatePullRequest
{
    public sealed class CreatePullRequestCommandHandler
    {
        private readonly AzureReposSettings _azureRepos;
        private readonly IPullRequestRepository _pullRequestRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly ISqlFileService _sqlFileService;

        public CreatePullRequestCommandHandler(AzureReposSettings azureRepos,
                                               IPullRequestRepository pullRequestRepository,
                                               ITicketRepository ticketRepository,
                                               ISqlFileService sqlFileService)
        {
            _azureRepos = azureRepos;
            _pullRequestRepository = pullRequestRepository;
            _ticketRepository = ticketRepository;
            _sqlFileService = sqlFileService;
        }

        public async Task<Result<int>> Handle(CreatePullRequestCommand createPullRequestCommand)
        {
            int ticketId = createPullRequestCommand.TicketId;
            var id = createPullRequestCommand.SqlFileId;
            
            
            var sqlFile = (await _ticketRepository.GetTicketAsync(ticketId))?
                .SqlFiles?
                .SingleOrDefault(sqlFile => 
                    sqlFile.sqlFileId == createPullRequestCommand.SqlFileId 
                    && sqlFile.pullRequestId == null); 
            
            if(sqlFile == null)
            {
                return Result.Failure<int>(new Error("PullRequest.Create", $"Sql file {createPullRequestCommand.SqlFileId:D2} don't existes for Ticket {createPullRequestCommand.TicketId:D8}"));
            }

            var getSqlFileResult = _sqlFileService.GetSqlFile(createPullRequestCommand.TicketId, createPullRequestCommand.SqlFileId);

            if (getSqlFileResult.IsFailure)
            {

                return Result.Failure<int>(new Error("PullRequest.Create", $"Script File don't for ticket {createPullRequestCommand.TicketId} and sqlFile {createPullRequestCommand.SqlFileId}"));
            }

            using StreamReader reader = new(getSqlFileResult.Value, Encoding.Latin1);            

            string content = reader.ReadToEnd();
            
            string sourceBranch = $"refs/heads/{ticketId}-{id}";
            const string targetBranch = "refs/heads/master";
            string commitComment = $"HD {ticketId} - {id}";
            string commitItemPath = $"/HD {ticketId} - {id}.sql";
            string commitContent = content;
            string pullRequestTitle = $"HD {ticketId} - {id}";
            string pullRequestDescription = $"HD {ticketId} - {id}";

            using var gitClient = GetGitClient();
            var repo = await gitClient.GetRepositoryAsync(_azureRepos.ProjName, _azureRepos.RepoName);
            var sourceBranchRef = GetSourceBranchRef(gitClient, repo, targetBranch);

            var gitPush = CreateGitPush(sourceBranch, sourceBranchRef.ObjectId, commitComment, commitItemPath, commitContent);
            var push = await gitClient.CreatePushAsync(gitPush, repo.Id);

            var gitPullRequest = CreateGitPullRequest(sourceBranch, targetBranch, pullRequestTitle, pullRequestDescription);
            var pullRequest = await gitClient.CreatePullRequestAsync(gitPullRequest, repo.Id);

            await UpdatePullRequest(gitClient, push, repo.Id, pullRequest.PullRequestId);

            var pullRequestResponse = new PullRequest()
            {
                Id = pullRequest.CodeReviewId,
                TicketId = ticketId
            };

            _pullRequestRepository.AddPullRequest(pullRequestResponse);
            return pullRequest.CodeReviewId;
        }

        private GitHttpClient GetGitClient()
        {
            VssConnection connection = new(new Uri(_azureRepos.UrlBase), new VssBasicCredential(string.Empty, _azureRepos.Token));
            return connection.GetClient<GitHttpClient>();
        }

        private GitRef GetSourceBranchRef(GitHttpClient gitClient,
                                          GitRepository repo,
                                          string targetBranch)
        {
            var refs = gitClient.GetRefsAsync(project: _azureRepos.ProjName, repositoryId: repo.Id).Result;
            return refs.First(r => r.Name == targetBranch);
        }

        private GitPush CreateGitPush(string sourceBranch,
                                      string objectId,
                                      string commitComment,
                                      string commitItemPath,
                                      string commitContent)
        {
            return new GitPush
            {
                RefUpdates = new[] { new GitRefUpdate { Name = sourceBranch, OldObjectId = objectId } },
                Commits = new[]
                {
                new GitCommitRef
                {
                    Comment = commitComment,
                    Changes = new[]
                    {
                        new GitChange
                        {
                            ChangeType = VersionControlChangeType.Add,
                            Item = new GitItem
                            {
                                Path = commitItemPath
                            },
                            NewContent = new ItemContent
                            {
                                ContentType = ItemContentType.RawText,
                                Content = commitContent
                            }
                        }
                    }
                }
            },
            };
        }

        private GitPullRequest CreateGitPullRequest(string sourceBranch,
                                                    string targetBranch,
                                                    string pullRequestTitle,
                                                    string pullRequestDescription)
        {
            return new GitPullRequest
            {
                SourceRefName = sourceBranch,
                TargetRefName = targetBranch,
                Title = pullRequestTitle,
                Description = pullRequestDescription,
            };
        }

        private async Task UpdatePullRequest(GitHttpClient gitClient,
                                             GitPush push,
                                             Guid repositoryId,
                                             int pullRequestId)
        {
            await gitClient.UpdatePullRequestAsync(new GitPullRequest()
            {
                AutoCompleteSetBy = push.PushedBy,
                CompletionOptions = new GitPullRequestCompletionOptions()
                {
                    TriggeredByAutoComplete = true,
                    DeleteSourceBranch = true,
                    MergeStrategy = GitPullRequestMergeStrategy.NoFastForward,
                }
            }, repositoryId, pullRequestId);
        }
    }
}
