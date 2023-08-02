using herodesknew.Application.Tickets.Queries.GetFilteredTickets;
using herodesknew.Domain.Repositories;
using herodesknew.Shared;

namespace herodesknew.Application.Tickets.Queries.GetTicket
{
    public sealed class GetTicketQueryHandler
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Result<TicketResponse>> Handle(GetTicketQuery getTicketQuery)
        {
            var ticket = await _ticketRepository.GetTicketAsync(getTicketQuery.TicketId);

            if (ticket == null)
            {
                return Result.Failure<TicketResponse>(Domain.Erros.DomainErrors.Ticket.NotFound(getTicketQuery.TicketId));
            }

            return Result.Success(ticket.MapTicketToTicketResponse());
        }
    }
}
