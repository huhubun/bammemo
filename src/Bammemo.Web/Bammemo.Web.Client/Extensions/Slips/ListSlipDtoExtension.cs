namespace Bammemo.Web.Client.Extensions.Slips;

public static class ListSlipDtoExtension
{
    public static string GetUrl(this ListSlipDto listSlipDto)
        => listSlipDto.FriendlyLinkName != null ? $"/p/{listSlipDto.FriendlyLinkName}" : $"/s/{listSlipDto.Id}";
}
