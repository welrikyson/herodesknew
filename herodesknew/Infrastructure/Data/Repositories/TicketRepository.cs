using Dapper;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Data.Repositories
{
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
            var query = """
                SELECT 
                    [id] ,
                    [uemail] ,
                    [title] ,
                    [description], 
                    [department],
                    [dbo].[fncConsumoSlaChamado]([id]) AS [slaUsed],
                    [status]
                FROM [dbo].[problems]
                WHERE [idAtuante] = @idUser and [status] in( 'AC', 'EA', 'OK') 
                ORDER BY [slaUsed] DESC
                """;
            var problemRows = await connection.QueryAsync(query, new { idSupportAgent });
            List<Ticket> tickets = new ();
            foreach (var problemRow in problemRows)
            {
                var ticket = new Ticket()
                {
                    Id = problemRow.id,
                    UserEmail = problemRow.uemail,
                    IdDepartment = problemRow.department,
                    Title = problemRow.title,
                    Description = problemRow.description,
                    SlaUsed = problemRow.slaUsed,
                    IsClosed = problemRow.status == "OK"
                };
                var queryAttachments = """
                    SELECT 
                        [COD_UPLOAD]
                        ,[ID]
                        ,[UPL_CAMINHO]
                        ,[UPL_NOME]
                    FROM [Helpdesk].[dbo].[TB_HLD_UPLOAD]
                    where [ID] = @id
                    order by COD_UPLOAD DESC 
                    """;

                var attachmentRows = await connection.QueryAsync(queryAttachments, new { id = ticket.Id });
                var attachments = attachmentRows.Select((attachment) =>
                {
                    return new Attachment()
                    {
                        Id = attachment.COD_UPLOAD,
                        FileName = attachment.UPL_NOME,
                        FilePath = attachment.UPL_CAMINHO,
                        TicketId = ticket.Id,
                    };
                });

                ticket.Attachments = attachments.Select(a => a).ToList();

                tickets.Add(ticket);
            }

            return tickets;

        }
    }
}
