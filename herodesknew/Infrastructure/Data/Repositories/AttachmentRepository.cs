using Dapper;
using herodesknew.Domain.Entities;
using herodesknew.Domain.Repositories;
using herodesknew.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static herodesknew.Infrastructure.Data.Repositories.TicketRepository;

namespace herodesknew.Infrastructure.Data.Repositories
{
    public sealed class AttachmentRepository : IAttachmentRepository
    {
        private HelpdeskContext _helpdeskContext;

        public AttachmentRepository(HelpdeskContext helpdeskContext)
        {
            _helpdeskContext = helpdeskContext;
        }

        public async Task<Attachment?> GetAttachmentBy(int attachmentID)
        {
            using var connection = _helpdeskContext.CreateDbConnection();

            string sql = $"""
                select 
                	[anexo].[COD_UPLOAD] AS [Id],
                    [anexo].[UPL_NOME] AS [FileName],
                    [anexo].[UPL_CAMINHO] AS [FilePath],
                	[anexo].[ID] AS [TicketId]

                from [dbo].[TB_HLD_UPLOAD] [anexo]

                where [anexo].[COD_UPLOAD] = @attachmentID;              
                """;
            var attachment = await connection.QuerySingleOrDefaultAsync<Attachment>(
                sql,
                new { attachmentID}
                );
            return attachment;
        }
    }
}
