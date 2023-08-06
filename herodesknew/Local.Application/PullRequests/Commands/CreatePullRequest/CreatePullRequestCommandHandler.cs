using herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile;
using herodesknew.Local.Domain.Utils;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace herodesknew.Local.Application.PullRequests.Commands.CreatePullRequest;

public sealed class CreatePullRequestCommandHandler
{
    public async Task<Result> Handle(CreatePullRequestCommand createPullRequestCommand)
    {
        try
        {
            var (Id, dirSltFullName, PathFullName, TicketId) = TicketFolderManager.GetTicketSubDirectoriesWithScripts(new[] { createPullRequestCommand.TicketId })
                    .SelectMany(item => item)
                    .Single(sqlFile => sqlFile.Id == createPullRequestCommand.SqlFileId);

            var path = Path.Combine(PathFullName, $"{TicketId}--{Id}.sql");

            if(SqlExecutionPlanDoc.GetPullRequestId(PathFullName) != null)
            {
                return Result.Failure(Error.NullValue);
            }

            if (!File.Exists(path))
            {
                return Result.Failure(Error.NullValue);
            }

            using HttpClient client = new() { BaseAddress = new("http://localhost:5000") };

            var responseMensage = await client.PostAsJsonAsync("/PullRequest/Create", new
            {
                TicketId = createPullRequestCommand.TicketId,
                Content = File.ReadAllText(path)
            });

            if(responseMensage.IsSuccessStatusCode)
            {
                var response  = await responseMensage.Content.ReadFromJsonAsync<CreatePullRequestResponse>();
                SqlExecutionPlanDoc.CreateDeployDocAsync(PathFullName, FileReader.ReadFirstLineFromFile(path)!, response!.PullRequestUrl);
                return Result.Success();                
            }
            else
            {
                return Result.Failure(Error.NullValue);
            }            
        }
        catch (ArgumentNullException)
        {
            return Result.Failure(Error.NullValue);
        }
    }
}

public record CreatePullRequestResponse(string PullRequestUrl);
