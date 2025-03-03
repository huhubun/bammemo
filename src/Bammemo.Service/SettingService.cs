using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Bammemo.Service;

public class SettingService(
    BammemoDbContext dbContext,
    IMemoryCache memoryCache) : ISettingService
{
    public async Task<Setting?> GetByKeyFromCacheAsync(string key)
        => await memoryCache.GetOrCreateAsync(GetCacheKey(key), async _ => await dbContext.Settings.AsNoTracking().SingleOrDefaultAsync(s => s.Key == key));

    public async Task<List<Setting>> GetByKeysAsync(IEnumerable<string> keys)
        => await dbContext.Settings.AsNoTracking().Where(s => keys.Contains(s.Key)).ToListAsync();

    public async Task CreateOrUpdateAsync(string key, string value)
    {
        var setting = await dbContext.Settings.SingleOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            await CreateAsync(key, value);
        }
        else
        {
            await UpdateAsync(key, value);
            memoryCache.Remove(GetCacheKey(key));
        }
    }

    public async Task CreateAsync(string key, string value)
    {
        await dbContext.Settings.AddAsync(new Setting
        {
            Key = key,
            Value = value,
            CreatedAt = DateTime.UtcNow.Ticks
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(string key, string value)
    {
        var entity = await dbContext.Settings.SingleAsync(s => s.Key == key);

        entity.Value = value;
        entity.UpdateAt = DateTime.UtcNow.Ticks;

        await dbContext.SaveChangesAsync();

        memoryCache.Remove(GetCacheKey(key));
    }

    private static string GetCacheKey(string key) => $"settings_{key.ToLower()}";
}
