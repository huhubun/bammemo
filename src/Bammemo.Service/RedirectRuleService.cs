using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Service;

public class RedirectRuleService(
    BammemoDbContext dbContext) : IRedirectRuleService
{
    public async Task<List<RedirectRule>> ListAsync()
        => await dbContext.RedirectRules.AsNoTracking().ToListAsync();

    public async Task<RedirectRule?> GetByIdAsync(int id)
        => await dbContext.RedirectRules.SingleOrDefaultAsync(r => r.Id == id);

    public async Task<RedirectRule?> GetBySourceAsync(string source)
    {
        return await dbContext.RedirectRules.AsNoTracking().SingleOrDefaultAsync(r => r.Source == source);
    }

    public async Task<RedirectRule> CreateAsync(RedirectRule redirectRule)
    {
        await dbContext.RedirectRules.AddAsync(redirectRule);
        await dbContext.SaveChangesAsync();

        return redirectRule;
    }

    public async Task<RedirectRule?> UpdateAsync(RedirectRule redirectRule)
    {
        dbContext.RedirectRules.Update(redirectRule);
        await dbContext.SaveChangesAsync();

        return redirectRule;
    }

    public async Task<int> DeleteAsync(int id)
    {
        return await dbContext.RedirectRules.Where(r => r.Id == id).ExecuteDeleteAsync();
    }
}
