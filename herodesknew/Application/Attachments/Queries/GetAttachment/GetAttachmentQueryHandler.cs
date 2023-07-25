using herodesknew.Domain.Erros;
using herodesknew.Domain.Repositories;
using herodesknew.Shared;
using System.Net.Mime;

namespace herodesknew.Application.Attachments.Queries.GetAttachment;

public sealed class GetAttachmentQueryHandler
{
    private readonly IAttachmentRepository _attachmentRepository;

    public GetAttachmentQueryHandler(IAttachmentRepository attachmentRepository)
    {
        _attachmentRepository = attachmentRepository;
    }

    public async Task<Result<AttachmentResponse>> Handle(GetAttachmentQuery getAttachmentQuery)
    {
        if (getAttachmentQuery.AttachmentId <= 0)
        {
            return Result.Failure<AttachmentResponse>(DomainErrors.Attachment.NotFound(getAttachmentQuery.AttachmentId));
        }

        var attachment = await _attachmentRepository.GetAttachmentBy(getAttachmentQuery.AttachmentId);

        if (attachment == null)
        {
            return Result.Failure<AttachmentResponse>(DomainErrors.Attachment.NotFound(getAttachmentQuery.AttachmentId));
        }

        string sharedFolderPath = @"Z:\";
        var anexoFilePath = Path.Combine(sharedFolderPath, attachment.FileName);

        if (!File.Exists(anexoFilePath))
        {
            return Result.Failure<AttachmentResponse>(DomainErrors.Attachment.NotFound(attachment.Id));
        }
        var contentType = Path.GetExtension(attachment.FileName).ToLowerInvariant() switch
        {
            ".txt" => MediaTypeNames.Text.Plain,
            ".pdf" => MediaTypeNames.Application.Pdf,
            ".jpg" or ".jpeg" => MediaTypeNames.Image.Jpeg,
            ".png" => "image/png",
            // Add more cases for other file types as needed
            _ => MediaTypeNames.Application.Octet,
        };

        FileStream fileStream = new FileStream(anexoFilePath, FileMode.Open, FileAccess.Read);

        return Result.Success(new AttachmentResponse() { ContentType = contentType, FileStream = fileStream, FileName = attachment.FileName });
    }
}
