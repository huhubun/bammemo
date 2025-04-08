using Bammemo.Service.Interfaces;
using Bammemo.Web.Client.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using System.Net.Mime;

namespace Bammemo.Web.MinimalApis;

public static class FileApis
{
    public static WebApplication MapFilesApi(this WebApplication app)
    {
        app.MapGet("/files/{*rest}", LoadFile).ExcludeFromDescription();

        return app;
    }

    private static async Task<IResult> LoadFile(
        [FromRoute] string rest, 
        [FromServices] IStorageService storageService, 
        [FromServices] ISlipService slipService,
        HttpContext context)
    {
        var fileMetadata = await storageService.GetFileMetadataByFullName(rest);
        if (fileMetadata == null)
        {
            return Results.NotFound();
        }

        if (fileMetadata.References?.Count > 0)
        {
            var slipIds = fileMetadata.References.Where(r => r.SourceType == (int)FileReferenceSourceType.Slip).Select(r => r.SourceId).ToArray();

            if (slipIds.Length > 0)
            {
                var status = await slipService.GetStatusAsync(slipIds);
                if (status.Any(s => s != SlipStatus.Public) && !(context.User.Identity?.IsAuthenticated ?? false))
                {
                    return Results.Unauthorized();
                }
            }
        }

        var inline = context.Request.Query.TryGetValue("response-content-disposition", out var fileHandler) && fileHandler == "inline";

        try
        {
            var result = await storageService.ReadAsync(fileMetadata);
            switch (result.Type)
            {
                case FileReadResultType.Stream:
                    var contentTypeProvider = new FileExtensionContentTypeProvider();
                    if (!contentTypeProvider.TryGetContentType(fileMetadata.FileName, out var contentType))
                    {
                        contentType = MediaTypeNames.Application.Octet;
                    }

                    var lastModified = new DateTimeOffset(fileMetadata.CreatedAt, TimeSpan.Zero);
                    var etag = new Microsoft.Net.Http.Headers.EntityTagHeaderValue($"\"{fileMetadata.HashValue}\"");

                    if (inline)
                    {
                        context.Response.Headers.Append("Content-Disposition", $"inline; filename={WebUtility.UrlEncode(fileMetadata.FileName)}");
                        return Results.Stream(result.Stream, contentType, null, lastModified, etag, true);
                    }
                    else
                    {
                        return Results.Stream(result.Stream, contentType, fileMetadata.FileName, lastModified, etag, true);
                    }
                case FileReadResultType.Url:
                    var url = UrlHelper.AppendQueryString(result.Url, KeyValuePair.Create("response-content-disposition", inline ? "inline" : "attachment")).ToString();
                    return Results.Redirect(url);
                default:
                    throw new NotSupportedException(result.Type.ToString());
            }
        }
        catch (Exception ex)
        {
            return Results.NotFound();

            // TODO LOG
        }
    }
}
