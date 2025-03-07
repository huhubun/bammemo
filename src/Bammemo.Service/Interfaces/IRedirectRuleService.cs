using Bammemo.Data.Entities;

namespace Bammemo.Service.Interfaces
{
    public interface IRedirectRuleService
    {
        Task<List<RedirectRule>> ListAsync();
        Task<RedirectRule?> GetByIdAsync(int id);
        Task<RedirectRule> CreateAsync(RedirectRule redirectRule);
        Task<RedirectRule?> GetBySourceAsync(string source);
        Task<RedirectRule?> UpdateAsync(RedirectRule redirectRule);
        Task<int> DeleteAsync(int id);
    }
}
