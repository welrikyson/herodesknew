using herodesknew.Domain.Services;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.SqlFiles.Commands.OpenSqlFile
{
    public sealed class OpenSqlFileCommandHandler
    {
        private readonly ISqlFileService _sqlFileService;

        public OpenSqlFileCommandHandler(ISqlFileService sqlFileService)
        {
            _sqlFileService = sqlFileService;
        }

        public Result Handle(OpenSqlFileCommand openSqlFileCommand )
        {
            return _sqlFileService.OpenFile(openSqlFileCommand.TicketId, openSqlFileCommand.SqlFileId);
        }
    }
}
