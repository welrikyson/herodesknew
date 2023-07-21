using Dapper;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Contexts;
using herodesknew.Infrastructure.Data.Mock;
using System.Diagnostics;

namespace herodesknew.Infrastructure.Data.Repositories;

internal sealed class TicketRepository : ITicketRepository
{
    private readonly HelpdeskContext _helpdeskContext;

    public TicketRepository(HelpdeskContext helpdeskContext)
    {
        _helpdeskContext = helpdeskContext;
    }

    public async Task<(List<Ticket>, int)> GetByIdSupportAgentAsync(int idSupportAgent, List<Filter>? filter, int skip, int take)
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
            new { idSupportAgent, skip,take, status = filterStatus?.Value }
        );

        var tickets = ticketQueryDatas
            .GroupBy(p => p.ProblemID)
            .Select(g => new Ticket
            {
                Id = g.Key,
                Description = g.First().ProblemDescription,
                IdDepartment = g.First().DepartmentID,
                SlaUsed = g.First().ProblemSLA,
                Title = g.First().ProblemTitle,
                UserEmail = g.First().ProblemEmail,
                StartDate = g.First().ProblemStartDate,
                CloseDate = g.First().ProblemCloseDate,
                Status = g.First().ProblemStatus.ParseStatus(),
                Attachments = g.Where(t => t.AttachmentID.HasValue)
                              .Select(t => new Attachment
                              {
                                  Id = t.AttachmentID ?? 0,
                                  FileName = t.AttachmentName ?? string.Empty,
                                  FilePath = t.AttachmentPath ?? string.Empty,
                                  TicketId = t.ProblemID
                              }).ToList()
            }).ToList();

        return (tickets, ticketQueryDatas?.FirstOrDefault()?.TotalCount ?? 0);
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
