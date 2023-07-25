using herodesknew.Domain.Entities;

namespace herodesknew.Infrastructure.Data.Mock
{
    public static class TicketMock
    {
        public static List<Ticket> GenerateTicketsMock()
        {
            var tickets = new List<Ticket>();

            for (int i = 1; i <= 16; i++)
            {
                var ticket = new Ticket
                {
                    Id = i,
                    IdDepartment = i % 4 + 1,
                    Title = $"Ticket {i}",
                    Description = $"Description for Ticket {i}",
                    UserEmail = $"user{i}@example.com",
                    SlaUsed = (i * 3) % 24,
                    Status = i % 2 == 0 ? StatusEnum.OK : StatusEnum.EA,
                    Attachments = GenerateAttachmentsMock(i),
                };

                tickets.Add(ticket);
            }

            return tickets;
        }

        private static IEnumerable<Attachment> GenerateAttachmentsMock(int ticketId)
        {
            var attachments = new List<Attachment>();

            for (int i = 1; i <= 3; i++)
            {
                var attachment = new Attachment
                {
                    Id = i,
                    FileName = $"Attachment{i}.txt",
                    FilePath = $"/attachments/{ticketId}/attachment{i}.txt",
                    TicketId = ticketId
                };

                attachments.Add(attachment);
            }

            return attachments;
        }
    }
}

