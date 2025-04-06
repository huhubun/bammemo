using Bammemo.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Bammemo.Service.Extensions;

public static class FileMetadataExtension
{
    public static Uri GetUrl(this FileMetadata fileMetadata, HttpRequest httpRequest)
    {
        var baseUriBuilder = httpRequest.Host.Port.HasValue
            ? new UriBuilder(httpRequest.Scheme, httpRequest.Host.Host, httpRequest.Host.Port.Value)
            : new UriBuilder(httpRequest.Scheme, httpRequest.Host.Host);

        return new Uri(baseUriBuilder.Uri, $"files/{fileMetadata.Path.Replace("\\", "/")}/{fileMetadata.FileName}");
    }
}
