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
    public sealed class GetMembersQueryHandler
    {
        private readonly ITicketRepository _ticketRepository;

        public GetMembersQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Result<(List<TicketResponse> ticketResponses, int totalCount)>> Handle(GetTicketsQuery getTicketsQuery)
        {
            (var tickets,var totalCount )= await _ticketRepository.GetByIdSupportAgentAsync(getTicketsQuery.IdSupportAgent, getTicketsQuery.Filters,getTicketsQuery.Skip,getTicketsQuery.Take);

            if (tickets.Count == 0)
            {
                return Result.Failure<(List<TicketResponse>, int totalCount)>(
                          DomainErrors.Member.NotExist);
            }

            List<TicketResponse> ticketReponseList = tickets.Select(MapTicketToTicketResponse).ToList();

            return Result.Success((ticketReponseList,totalCount));

            static TicketResponse MapTicketToTicketResponse(Domain.Entities.Ticket ticket)
            =>
                new () 
                { 
                    Id = ticket.Id,
                    Description = ticket.Description,
                    SlaUsed = ticket.SlaUsed,
                    Title = ticket.Title,
                    UserEmail = ticket.UserEmail,
                    Attachments = ticket.Attachments,
                    CloseDate = ticket.CloseDate,
                    StartDate = ticket.StartDate,
                    Status = ticket.Status,
                    Solutions = ticket.Solutions,                    
                };            
        }
    }
}
