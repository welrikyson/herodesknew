using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Repositories
{
    public interface ISqlFileRepository
    {
        IEnumerable<(string sqlFileName, int? pullRequestId)> GetSqlFiles(int ticketId);
    }
}
