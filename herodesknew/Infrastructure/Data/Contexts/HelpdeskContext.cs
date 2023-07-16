using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace herodesknew.Infrastructure.Contexts
{
    public class HelpdeskContext
    {
        private readonly string HelpdeskConnectionString;

        public HelpdeskContext(IConfiguration configuration)
        {
            HelpdeskConnectionString = configuration.GetConnectionString("HelpdeskSqlConnection")!;
        }

        public IDbConnection CreateDbConnection() => new SqlConnection(HelpdeskConnectionString);
    }
}
