using DocumentFormat.OpenXml.Office2010.Excel;
using herodesknew.Domain.Repositories;
using herodesknew.Local.Application;
using herodesknew.Local.Application.SqlFiles.Queries.GetSqlFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Infrastructure.Data.Repositories
{
    public class LocalSqlFileRepository : ISqlFileRepository
    {
        private readonly GetSqlFileQueryHandler _getSqlFileQueryHandler;

        public LocalSqlFileRepository(GetSqlFileQueryHandler getSqlFileQueryHandler)
        {
            _getSqlFileQueryHandler = getSqlFileQueryHandler;
        }
        public IEnumerable<(string sqlFileName,int? pullRequestId)> GetSqlFiles(int ticketId)
        {
            return _getSqlFileQueryHandler.Handle(ticketId);
        }
    }
}
