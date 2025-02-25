using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Service.Server;

public class SettingService(
    BammemoDbContext dbContext) : ISettingService
{
    public async Task<Setting?> GetByKeyAsync(string key)
        => await dbContext.Settings.AsNoTracking().SingleOrDefaultAsync(s => s.Key == key);

    public async Task<List<Setting>> GetByKeysAsync(IEnumerable<string> keys)
        => await dbContext.Settings.AsNoTracking().Where(s => keys.Contains(s.Key)).ToListAsync();

    public async Task CreateOrUpdateAsync(string key, string value)
    {
        var setting = await GetByKeyAsync(key);
        if (setting == null)
        {
            await CreateAsync(key, value);
        }
        else
        {
            await UpdateAsync(key, value);
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
    }
}
