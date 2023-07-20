using Dapper;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Contexts;

namespace herodesknew.Infrastructure.Data.Repositories;


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
}
internal sealed class TicketRepository : ITicketRepository
{
    private readonly HelpdeskContext helpdeskContext;

    public TicketRepository(HelpdeskContext helpdeskContext)
    {
        this.helpdeskContext = helpdeskContext;
    }

    public async Task<List<Ticket>> GetByIdSupportAgentAsync(int idSupportAgent)
    {            
        using var connection = helpdeskContext.CreateDbConnection();
        string sql = """
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
                    [anexo].[ID] AS [AttachmentID],
                    [anexo].[UPL_NOME] AS [AttachmentName],
                    [anexo].[UPL_CAMINHO] AS [AttachmentPath]
                FROM 
                    [dbo].[problems] [p]
                LEFT JOIN 
                    [dbo].[TB_HLD_UPLOAD] [anexo] ON [p].[id] = [anexo].[ID]
                WHERE 
                    [p].[idAtuante] = @idSupportAgent
                ORDER BY 
                    [p].[id] DESC;                
                """;

        var ticketQueryDatas = await connection.QueryAsync<TicketQueryData>(
            sql,
            new { idSupportAgent }
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

        return tickets;
    }
}
