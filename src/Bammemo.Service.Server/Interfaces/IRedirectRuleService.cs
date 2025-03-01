using Bammemo.Data.Entities;

namespace Bammemo.Service.Server.Interfaces
{
    public interface IRedirectRuleService
    {
        Task<RedirectRule?> GetBySourceAsync(string source);
    }
}
