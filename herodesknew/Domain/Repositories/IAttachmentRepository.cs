using herodesknew.Domain.Entities;

namespace herodesknew.Domain.Repositories;

public interface IAttachmentRepository
{
    Task<Attachment?> GetAttachmentBy(int attachmentID);
    Task<Attachment?> GetAttachmentBy(string attachementName);
}
