using Bammemo.Data.Entities;

namespace Bammemo.Service.Interfaces
{
    public interface IRedirectRuleService
    {
        Task<RedirectRule?> GetBySourceAsync(string source);
    }
}
