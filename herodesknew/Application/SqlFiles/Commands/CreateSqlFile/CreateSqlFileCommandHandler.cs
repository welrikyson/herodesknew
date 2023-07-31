using herodesknew.Domain.Services;
using herodesknew.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.SqlFiles.Commands.CreateSqlFile
{
    public sealed class CreateSqlFileCommandHandler
    {
        private readonly ISqlFileService _sqlFileService;

        public CreateSqlFileCommandHandler(ISqlFileService sqlFileService)
        {
            _sqlFileService = sqlFileService;
        }
        public Result Handle(CreateSqlFileCommand createSqlFileCommand)
        {
            return _sqlFileService.CreateSqlFile(createSqlFileCommand.ticketId);
        }
    }
}
