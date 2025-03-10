using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Bammemo.Service;

public class SiteLinkService(
    BammemoDbContext dbContext,
    IMemoryCache memoryCache) : ISiteLinkService
{
    public async Task<SiteLink[]> ListFromCacheAsync()
        => await memoryCache.GetOrCreateAsync(GetCacheKey(), async _ => await dbContext.SiteLinks.AsNoTracking().ToArrayAsync()) ?? [];

    public async Task<SiteLink?> GetByIdAsync(int id)
        => await dbContext.SiteLinks.SingleOrDefaultAsync(r => r.Id == id);

    public async Task<SiteLink> CreateAsync(SiteLink siteLink)
    {
        siteLink.CreatedAt = DateTime.UtcNow.Ticks;
        siteLink.UpdateAt = DateTime.UtcNow.Ticks;

        await dbContext.SiteLinks.AddAsync(siteLink);
        await dbContext.SaveChangesAsync();

        memoryCache.Remove(GetCacheKey());

        return siteLink;
    }

    public async Task UpdateAsync(SiteLink siteLink)
    {
        siteLink.UpdateAt = DateTime.UtcNow.Ticks;

        dbContext.SiteLinks.Update(siteLink);
        await dbContext.SaveChangesAsync();

        memoryCache.Remove(GetCacheKey());
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rows = await dbContext.SiteLinks.Where(r => r.Id == id).ExecuteDeleteAsync();

        memoryCache.Remove(GetCacheKey());

        return rows;
    }

    private static string GetCacheKey() => $"sitelinks";
}
