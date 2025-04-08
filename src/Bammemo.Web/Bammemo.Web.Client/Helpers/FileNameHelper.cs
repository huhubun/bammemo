namespace Bammemo.Web.Client.Helpers;

public static class FileNameHelper
{
    private static readonly string[] _imageFileExtensions = [".jpg", ".png", ".gif", ".webp"];

    public static bool IsImage(string fileName)
    {
        if (fileName == null)
        {
            return false;
        }

        var fileInfo = new FileInfo(fileName);
        return _imageFileExtensions.Contains(fileInfo.Extension);
    }
}
