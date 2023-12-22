namespace herodesknew.Application.Attachments.Queries.GetAttachment
{
    public class AttachmentResponse
    {
        public required FileStream FileStream { get; set; }
        public required string ContentType { get; set; }

        public required string FileName { get; set; }
    }
}
