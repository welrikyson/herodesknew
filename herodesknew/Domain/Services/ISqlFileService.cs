using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Services
{
    public interface ISqlFileService
    {
        Result CreateSqlFile(int ticketId);
        Result OpenFile(int ticketId, int sqlFileId);
    }
}
