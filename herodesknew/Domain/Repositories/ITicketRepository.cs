using herodesknew.Domain.Entities;

namespace herodesknew.Domain.Repositories
{
    public interface ITicketRepository
    {
        Task<(List<Ticket> tickets, int totalCount)> GetFilteredTicketsAsync(int idSupportAgent, List<Filter>? filter, int skip, int take);
        Task<Ticket?> GetTicketAsync(int ticketId);
    }

    public class Filter
    {
        public required string Value { get; set; }
        public required string Operator { get; set; }
        public required string Property { get; set; }
    }
}
