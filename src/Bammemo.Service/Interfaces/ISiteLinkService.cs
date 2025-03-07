using Bammemo.Data.Entities;

namespace Bammemo.Service.Interfaces;

public interface ISiteLinkService
{
    Task<SiteLink[]> ListFromCacheAsync();
    Task<SiteLink?> GetByIdAsync(int id);
    Task<SiteLink> CreateAsync(SiteLink siteLink);
    Task UpdateAsync(SiteLink siteLink);
    Task<int> DeleteAsync(int id);
}