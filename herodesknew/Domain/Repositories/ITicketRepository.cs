﻿using herodesknew.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Domain.Repositories
{
    public interface ITicketRepository
    {
        Task<(List<Ticket> tickets, int totalCount)> GetByIdSupportAgentAsync(int idSupportAgent, List<Filter>? filter, int skip, int take);
    }

    public class Filter
    {
        public required string Value { get; set; }
        public required string Operator { get; set; }
        public required string Property { get; set; }
    }
}
