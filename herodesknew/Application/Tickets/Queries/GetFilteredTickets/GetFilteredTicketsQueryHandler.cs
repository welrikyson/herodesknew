using herodesknew.Domain.Erros;
using herodesknew.Domain.Repositories;
using herodesknew.Shared;

namespace herodesknew.Application.Tickets.Queries.GetFilteredTickets;

public sealed class GetFilteredTicketsQueryHandler
{
    private readonly ITicketRepository _ticketRepository;

    public GetFilteredTicketsQueryHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<Result<(List<TicketResponse> ticketResponses, int totalCount)>> Handle(GetFilteredTicketsQuery getTicketsQuery)
    {
        (var tickets, var totalCount) = await _ticketRepository.GetFilteredTicketsAsync(getTicketsQuery.IdSupportAgent,
                                                                                        getTicketsQuery.Filters,
                                                                                        getTicketsQuery.Skip,
                                                                                        getTicketsQuery.Take);

        if (tickets.Count == 0)
        {
            return Result.Failure<(List<TicketResponse>, int totalCount)>(DomainErrors.Member.NotExist);
        }

        List<TicketResponse> ticketReponseList = tickets.Select(TicketMapper.MapTicketToTicketResponse).ToList();

        return Result.Success((ticketReponseList, totalCount));

    }
}

public static class TicketMapper
{
    public static TicketResponse MapTicketToTicketResponse(this Domain.Entities.Ticket ticket)
    =>
        new()
        {
            Id = ticket.Id,
            Description = ticket.Description,
            SlaUsed = ticket.SlaUsed,
            Title = ticket.Title,
            UserEmail = ticket.UserEmail,
            Attachments = ticket.Attachments,
            PullRequests = ticket.PullRequests,
            CloseDate = ticket.CloseDate,
            StartDate = ticket.StartDate,
            Status = ticket.Status,
        };
}
