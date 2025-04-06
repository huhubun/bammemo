using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net.Mime;

namespace Bammemo.Web.MinimalApis;

public static class FileApis
{
    public static WebApplication MapFilesApi(this WebApplication app)
    {
        app.MapGet("/files/{*rest}", LoadFile).ExcludeFromDescription();

        return app;
    }

    private static async Task<IResult> LoadFile([FromRoute] string rest, [FromServices] IStorageService storageService)
    {
        var fileMetadata = await storageService.GetFileMetadataByFullName(rest);
        if (fileMetadata == null)
        {
            return Results.NotFound();
        }

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

                return Results.Stream(result.Stream, contentType, fileMetadata.FileName, lastModified, etag, true);
            case FileReadResultType.Url:
                return Results.Redirect(result.Url);
            default:
                throw new NotSupportedException(result.Type.ToString());
        }
    }
}
