using Bammemo.Data.Entities;

namespace Bammemo.Service.Server.Interfaces;
public interface ISettingService
{
    Task<Setting?> GetByKeyAsync(string key);
    Task CreateAsync(string key, string value);
    Task CreateOrUpdateAsync(string key, string value);
    Task UpdateAsync(string key, string value);
}