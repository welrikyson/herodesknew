using Dapper;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace herodesknew.Infrastructure.Data.Repositories;

internal sealed class TicketRepository : ITicketRepository
{
    private readonly HelpdeskContext _helpdeskContext;
    private readonly HerodesknewDbContext _herodesknewDbContext;

    public TicketRepository(HelpdeskContext helpdeskContext, HerodesknewDbContext herodesknewDbContext)
    {
        _helpdeskContext = helpdeskContext;
        _herodesknewDbContext = herodesknewDbContext;
    }

    public async Task<(List<Ticket>, int)> GetFilteredTicketsAsync(int idSupportAgent, List<Filter>? filter, int skip, int take)
    {
        using var connection = _helpdeskContext.CreateDbConnection();
        var filterStatus =
            filter?
                .Where(f => f.Property == nameof(Ticket.Status) &&
                            Enum.TryParse<StatusEnum>(f.Value, out var _))
                .Select((f) => (Value: Enum.GetName(Enum.Parse<StatusEnum>(f.Value)), Condition: "AND Status = @status"))
                .SingleOrDefault();

        string sql = $"""
                SELECT
                    [p].[id] AS [ProblemID],
                    [p].[title] AS [ProblemTitle],
                    [p].[description] AS [ProblemDescription],
                    [p].[uemail] AS [ProblemEmail],
                    [p].[department] AS [DepartmentID],
                    [p].[Status] AS [ProblemStatus],
                    [p].[start_date] AS [ProblemStartDate],
                    [p].[close_date] AS [ProblemCloseDate],
                    [dbo].[fncConsumoSlaChamado]([p].[id]) AS [ProblemSLA],
                    [anexo].[COD_UPLOAD] AS [AttachmentID],
                    [anexo].[UPL_NOME] AS [AttachmentName],
                    [anexo].[UPL_CAMINHO] AS [AttachmentPath],
                	[top10].[TotalCount]
                FROM 
                    (
                        SELECT DISTINCT [id], COUNT(*) OVER() AS [TotalCount]
                        FROM [dbo].[problems]
                        WHERE [idAtuante] = @idSupportAgent {filterStatus?.Condition ?? string.Empty}
                        ORDER BY [id] DESC		
                		OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY
                    ) AS top10
                JOIN [dbo].[problems] [p] ON [top10].[id] = [p].[id]
                LEFT JOIN [dbo].[TB_HLD_UPLOAD] [anexo] ON [p].[id] = [anexo].[ID]
                ORDER BY [p].[id] DESC;                
                """;

        var ticketQueryDatas = await connection.QueryAsync<TicketQueryData>(
            sql,
            new { idSupportAgent, skip, take, status = filterStatus?.Value }
        );

        var ticketIds = ticketQueryDatas.Select(t => t.ProblemID).Distinct().ToHashSet();

        // Fetch PullRequests data for all relevant tickets.
        var pullRequestsData = await _herodesknewDbContext.PullRequests
            .Where(p => ticketIds.Contains(p.TicketId))
            .ToListAsync();

        // Organize PullRequests data into a dictionary for efficient retrieval.
        var pullRequestsDictionary = pullRequestsData
            .GroupBy(p => p.TicketId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Build the Ticket objects and include the corresponding PullRequests.
        var tickets = ticketQueryDatas
            .GroupBy(p => p.ProblemID)
            .Select(g => new Ticket
            {
                Id = g.First().ProblemID,
                Description = g.First().ProblemDescription,
                IdDepartment = g.First().DepartmentID,
                SlaUsed = g.First().ProblemSLA,
                Title = g.First().ProblemTitle,
                UserEmail = g.First().ProblemEmail,
                StartDate = g.First().ProblemStartDate,
                CloseDate = g.First().ProblemCloseDate,
                Status = g.First().ProblemStatus.ParseStatus(),
                Attachments = g.First().AttachmentID.HasValue
                           ? new List<Attachment>
                           {
                            new Attachment
                            {
                                Id = g.First().AttachmentID!.Value,
                                FileName = g.First().AttachmentName ?? string.Empty,
                                FilePath = g.First().AttachmentPath ?? string.Empty,
                                TicketId = g.First().ProblemID
                            }
                           }
                           : new List<Attachment>(),
                PullRequests = pullRequestsDictionary.ContainsKey(g.First().ProblemID)
                           ? pullRequestsDictionary[g.First().ProblemID]
                           : new List<PullRequest>(),

            });

        return (tickets.ToList(), ticketQueryDatas?.FirstOrDefault()?.TotalCount ?? 0);
    }

    public async Task<Ticket?> GetTicketAsync(int ticketId)
    {
        using var connection = _helpdeskContext.CreateDbConnection();

        string sql = $"""
                SELECT
                    [p].[id] AS [ProblemID],
                    [p].[title] AS [ProblemTitle],
                    [p].[description] AS [ProblemDescription],
                    [p].[uemail] AS [ProblemEmail],
                    [p].[department] AS [DepartmentID],
                    [p].[Status] AS [ProblemStatus],
                    [p].[start_date] AS [ProblemStartDate],
                    [p].[close_date] AS [ProblemCloseDate],
                    [dbo].[fncConsumoSlaChamado]([p].[id]) AS [ProblemSLA],
                    [anexo].[COD_UPLOAD] AS [AttachmentID],
                    [anexo].[UPL_NOME] AS [AttachmentName],
                    [anexo].[UPL_CAMINHO] AS [AttachmentPath],
                	COUNT(*) OVER() AS [TotalCount]
                FROM [dbo].[problems] [p]
                    LEFT JOIN [dbo].[TB_HLD_UPLOAD] [anexo] ON [p].[id] = [anexo].[ID]
                WHERE [P].[id] = @ticketId;                
                """;

        var ticketQueryDatas = await connection.QueryAsync<TicketQueryData>(
            sql,
            new { ticketId }
        );

        var ticketIds = ticketQueryDatas.Select(t => t.ProblemID).Distinct().ToHashSet();

        // Fetch PullRequests data for all relevant tickets.
        var pullRequestsData = await _herodesknewDbContext.PullRequests
            .Where(p => ticketIds.Contains(p.TicketId))
            .ToListAsync();

        // Organize PullRequests data into a dictionary for efficient retrieval.
        var pullRequestsDictionary = pullRequestsData
            .GroupBy(p => p.TicketId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Build the Ticket objects and include the corresponding PullRequests.
        var tickets = ticketQueryDatas
            .GroupBy(p => p.ProblemID)
            .Select(g => new Ticket
            {
                Id = g.First().ProblemID,
                Description = g.First().ProblemDescription,
                IdDepartment = g.First().DepartmentID,
                SlaUsed = g.First().ProblemSLA,
                Title = g.First().ProblemTitle,
                UserEmail = g.First().ProblemEmail,
                StartDate = g.First().ProblemStartDate,
                CloseDate = g.First().ProblemCloseDate,
                Status = g.First().ProblemStatus.ParseStatus(),
                Attachments = g.First().AttachmentID.HasValue
                           ? new List<Attachment>
                           {
                            new Attachment
                            {
                                Id = g.First().AttachmentID!.Value,
                                FileName = g.First().AttachmentName ?? string.Empty,
                                FilePath = g.First().AttachmentPath ?? string.Empty,
                                TicketId = g.First().ProblemID
                            }
                           }
                           : new List<Attachment>(),
                PullRequests = pullRequestsDictionary.ContainsKey(g.First().ProblemID)
                           ? pullRequestsDictionary[g.First().ProblemID]
                           : new List<PullRequest>(),
            });

        return tickets.SingleOrDefault();
    }
    public class TicketQueryData
    {
        public int ProblemID { get; set; }
        public required string ProblemEmail { get; set; }
        public required string ProblemTitle { get; set; }
        public required string ProblemDescription { get; set; }
        public required int ProblemSLA { get; set; }
        public required string ProblemStatus { get; set; }

        public DateTime ProblemStartDate { get; set; }

        public DateTime ProblemCloseDate { get; set; }

        public required int DepartmentID { get; set; }

        public int? AttachmentID { get; set; }
        public string? AttachmentPath { get; set; }
        public string? AttachmentName { get; set; }
        public int TotalCount { get; set; }
    }
}
