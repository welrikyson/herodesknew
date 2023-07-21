using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.Tickets.Queries.GetTickets
{
    public class GetTicketsQuery
    {
        public List<Domain.Repositories.Filter>? Filters { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }    
}
