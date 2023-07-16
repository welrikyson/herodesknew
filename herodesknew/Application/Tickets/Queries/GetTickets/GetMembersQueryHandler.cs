using Gatherly.Domain.Shared;
using herodesknew.Domain.Erros;
using herodesknew.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace herodesknew.Application.Tickets.Queries.GetTickets
{
    internal sealed class GetMembersQueryHandler
    {
        private readonly ITicketRepository _ticketRepository;

        public GetMembersQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Result<List<TicketResponse>>> Handle(GetTicketsQuery getTicketsQuery)
        {
            var tickets = await _ticketRepository.GetByIdSupportAgentAsync(1456);

            if (tickets.Count == 0)
            {
                return Result.Failure<List<TicketResponse>>(
                          DomainErrors.Member.NotExist);
            }

            List<TicketResponse> ticketReponseList = tickets.Select(ticket => new TicketResponse())                                        
                                                            .ToList();
            
            return Result.Success(ticketReponseList);
        }
    }
}
