using herodesknew.Application.Attachments.Queries.GetAttachment;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace herodesknew.Server.Controllers;


[ApiController]
[Route("[controller]")]
[EnableCors("AllowAllOrigins")]
public sealed class AttachmentController : Controller
{
    private readonly GetAttachmentQueryHandler getAttachmentQueryHandler;

    public AttachmentController(GetAttachmentQueryHandler getAttachmentQueryHandler)
    {
        this.getAttachmentQueryHandler = getAttachmentQueryHandler;
    }
    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetAttachmentAsync(string fileName)
    {
        var result =  await getAttachmentQueryHandler.Handle(new () { AttachmentFileName = fileName.ReplaceLineEndings().TrimEnd() });

        return result.IsSuccess ? File(result.Value.FileStream, result.Value.ContentType) : BadRequest(result.Error);

    }
}