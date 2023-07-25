namespace herodesknew.Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public int TicketId { get; set; }
    }
}
