using Bammemo.Service.Extensions;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Storages;
using Bammemo.Web.WebApiModels.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Bammemo.Web.Controllers;

[Route("api/files")]
[ApiController]
public class FileController(
    IStorageService storageService) : BammemoControllerBase
{
    [Authorize]
    [HttpPost(""), Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType<UploadFileResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UploadFileAsync([FromForm] UploadFileRequest request)
    {
        using var stream = request.File.OpenReadStream();

        var fileMetadata = await storageService.SaveAsync(request.File.FileName, request.Type, stream);

        return Ok(new UploadFileResponse
        {
            FileMetadataId = fileMetadata.Id,
            FileName = fileMetadata.FileName,
            Url = fileMetadata.GetUrl(Request).ToString()
        });
    }

    [Authorize]
    [HttpDelete("{fileMetadataId:int}"), Consumes(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteFileAsync([FromRoute] int fileMetadataId)
    {
        var result = await storageService.DeleteAsync(fileMetadataId);
        if(result.Status == FileDeleteStatus.Deleted)
        {
            return NoContent();
        }
        else if(result.Status == FileDeleteStatus.FileMetadataNotFound)
        {
            return NotFound();
        }

        return BadRequest();
    }
}
