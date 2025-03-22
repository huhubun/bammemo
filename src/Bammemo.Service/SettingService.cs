using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Bammemo.Service;

public class SettingService(
    BammemoDbContext dbContext,
    IMemoryCache memoryCache,
    ISecurityService securityService) : ISettingService
{
    public async Task<Setting?> GetByKeyFromCacheAsync(string key)
        => await memoryCache.GetOrCreateAsync(GetCacheKey(key), async _ => await GetByKeyAsync(key));

    public async Task<Setting?> GetByKeyAsync(string key, bool tracking = false)
    {
        IQueryable<Setting> query = dbContext.Settings;

        if (!tracking)
        {
            query = query.AsNoTracking();
        }

        var result = await query.SingleOrDefaultAsync(s => s.Key == key);

        if (result != null && SettingKeys.CheckProtectedSettingByKey(result.Key))
        {
            result.Value = result.Value != null ? securityService.Decrypt(result.Value) : null;
        }

        return result;
    }

    public async Task<List<Setting>> GetByKeysAsync(IEnumerable<string> keys)
    {
        var result = await dbContext.Settings.AsNoTracking().Where(s => keys.Contains(s.Key)).ToListAsync();

        foreach (var i in result)
        {
            if (SettingKeys.CheckProtectedSettingByKey(i.Key))
            {
                i.Value = i.Value != null ? securityService.Decrypt(i.Value) : null;
            }
        }

        return result;
    }

    public async Task CreateOrUpdateAsync(string key, string? value)
    {
        var setting = await dbContext.Settings.SingleOrDefaultAsync(s => s.Key == key);
        if (setting == null)
        {
            await CreateAsync(key, value);
        }
        else
        {
            await UpdateAsync(key, value);
        }

        memoryCache.Remove(GetCacheKey(key));
    }

    public async Task CreateAsync(string key, string? value)
    {
        if (SettingKeys.CheckProtectedSettingByKey(key))
        {
            value = value != null ? securityService.Encrypt(value) : null;
        }

        await dbContext.Settings.AddAsync(new Setting
        {
            Key = key,
            Value = value,
            CreatedAt = DateTime.UtcNow.Ticks,
            UpdateAt = DateTime.UtcNow.Ticks
        });

        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(string key, string? value)
    {
        if (SettingKeys.CheckProtectedSettingByKey(key))
        {
            value = value != null ? securityService.Encrypt(value) : null;
        }

        var entity = await dbContext.Settings.SingleAsync(s => s.Key == key);

        entity.Value = value;
        entity.UpdateAt = DateTime.UtcNow.Ticks;

        await dbContext.SaveChangesAsync();

        memoryCache.Remove(GetCacheKey(key));
    }

    public async Task DeleteAsync(Setting setting)
    {
        dbContext.Settings.Remove(setting);
        await dbContext.SaveChangesAsync();

        memoryCache.Remove(GetCacheKey(setting.Key));
    }

    private static string GetCacheKey(string key) => $"settings_{key.ToLower()}";
}
