using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Service
{
    public class RedirectRuleService(
        BammemoDbContext dbContext) : IRedirectRuleService
    {
        public async Task<RedirectRule?> GetBySourceAsync(string source)
        {
            return await dbContext.RedirectRules.AsNoTracking().SingleOrDefaultAsync(r => r.Source == source);
        }
    }
}
