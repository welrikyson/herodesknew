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
        if (string.IsNullOrEmpty(getAttachmentQuery.AttachmentFileName))
        {
            return Result.Failure<AttachmentResponse>(DomainErrors.Attachment.NotFound(getAttachmentQuery.AttachmentFileName));
        }

        var attachment = await _attachmentRepository.GetAttachmentBy(getAttachmentQuery.AttachmentFileName);

        if (attachment == null)
        {
            return Result.Failure<AttachmentResponse>(DomainErrors.Attachment.NotFound(getAttachmentQuery.AttachmentFileName));
        }

        const string sharedFolderPath = @"Z:\";
        var anexoFilePath = Path.Combine(sharedFolderPath, attachment.FileName);

        if (!File.Exists(anexoFilePath))
        {
            return Result.Failure<AttachmentResponse>(DomainErrors.Attachment.NotFound(attachment.FileName));
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
