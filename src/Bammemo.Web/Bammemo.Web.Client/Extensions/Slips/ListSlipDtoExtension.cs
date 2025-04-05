using Bammemo.Service.Abstractions.Dtos.Slips;

namespace Bammemo.Web.Client.Extensions.Slips;

public static class ListSlipDtoExtension
{
    public static string GetUrl(this ListSlipDto listSlipDto)
        => String.IsNullOrEmpty(listSlipDto.FriendlyLinkName) ? $"/s/{listSlipDto.Id}" : $"/p/{listSlipDto.FriendlyLinkName}";
}
