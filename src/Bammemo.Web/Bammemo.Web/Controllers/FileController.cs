using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.Files;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Bammemo.Web.Controllers;

[Route("api/files")]
[ApiController]
public class FileController(
    IStorageService storageService) : BammemoControllerBase
{
    [HttpPost(""), Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType<UploadFileResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileRequest request)
    {
        using var stream = request.File.OpenReadStream();

        var fileMetadata = await storageService.SaveAsync(request.File.FileName, request.Type, stream, request.KeepFileName);
        var baseUriBuilder = Request.Host.Port.HasValue
            ? new UriBuilder(Request.Scheme, Request.Host.Host, Request.Host.Port.Value)
            : new UriBuilder(Request.Scheme, Request.Host.Host);

        return Ok(new UploadFileResponse
        {
            FileName = fileMetadata.FileName,
            Url = new Uri(baseUriBuilder.Uri, $"files/{fileMetadata.Path.Replace("\\", "/")}/{fileMetadata.FileName}").ToString()
        });
    }
}
