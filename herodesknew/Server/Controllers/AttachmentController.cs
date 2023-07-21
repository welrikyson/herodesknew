using Azure.Core;
using herodesknew.Application.Attachments.Queries.GetAttachment;
using herodesknew.Application.Tickets.Queries.GetTickets;
using herodesknew.Domain.Repositories;
using herodesknew.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radzen.Blazor;
using System.IO;
using System.Net.Mime;

namespace herodesknew.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly ILogger<AttachmentController> _logger;
        private readonly GetAttachmentQueryHandler _getAttachmentQueryHandler;

        public AttachmentController(ILogger<AttachmentController> logger,
                                GetAttachmentQueryHandler getAttachmentQueryHandler)
        {
            _logger = logger;
            _getAttachmentQueryHandler = getAttachmentQueryHandler;
        }

        [HttpGet("{attachmentId}")]
        public async Task<IActionResult> GetAsync(int attachmentId)
        {
            var result = await _getAttachmentQueryHandler.Handle(new() { AttachmentId = attachmentId });
            if (result.IsSuccess)
            {   
                return File(result.Value.FileStream,result.Value.ContentType, result.Value.FileName.Trim());
            }
            else
            {
                return NotFound();
            }            
        }              
    }
}
