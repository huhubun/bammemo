using Bammemo.Data.Entities;

namespace Bammemo.Service.Interfaces;

public interface ISettingService
{
    Task<Setting?> GetByKeyFromCacheAsync(string key);
    Task<List<Setting>> GetByKeysAsync(IEnumerable<string> keys);
    Task CreateAsync(string key, string? value);
    Task CreateOrUpdateAsync(string key, string? value);
    Task UpdateAsync(string key, string? value);
    Task DeleteAsync(Setting setting);
}