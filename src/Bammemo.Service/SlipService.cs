using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Enums;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Helpers;
using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Service;

public class SlipService(
    BammemoDbContext dbContext,
    IHttpContextAccessor httpContext) : ISlipService
{
    public async Task<Slip[]> ListAsync(
        ListSlipQueryRequest? query,
        CursorPagingRequest<int>? paging)
    {
        IQueryable<Slip> slips = dbContext.Slips.AsNoTracking().OrderByDescending(s => s.Id);

        if (query != null)
        {
            if (query.StartTime.HasValue && query.EndTime.HasValue)
            {
                slips = slips.Where(s => s.CreatedAt >= query.StartTime.Value)
                    .Where(s => s.CreatedAt < query.EndTime.Value);
            }

            if (query.Tags?.Length > 0)
            {
                slips = slips.Where(s => s.Tags.Any());

                foreach (var tag in query.Tags)
                {
                    slips = slips.Where(s => s.Tags.Select(t => t.Tag).Contains(tag));
                }
            }
        }

        if (query?.Status == null)
        {
            slips = slips.Where(s => s.Status == (int)SlipStatus.Public);
        }
        else
        {
            var needAuthorizeSlipStatus = GetNeedAuthorizeSlipStatus(query.Status);
            if (needAuthorizeSlipStatus.Any() && !IsAuthenticated)
            {
                throw new UnauthorizedAccessException($"Slip status: {String.Join(", ", needAuthorizeSlipStatus.Cast<SlipStatus>())}.");
            }

            slips = slips.Where(s => query.Status.Contains(s.Status));
        }

        var take = paging?.Take ?? 5;

        if (paging != null)
        {
            slips = slips.Where(s => s.Id < paging.Cursor);
        }

        return await slips.Take(take).Include(s => s.Tags).ToArrayAsync();
    }

    public async Task<Slip?> GetByIdAsync(int id)
    {
        var slip = await dbContext.Slips.SingleOrDefaultAsync(s => s.Id == id);

        SlipAuthorizeGuardian(slip);

        return slip;
    }

    public async Task<Slip?> GetByIdNoTrackingAsync(int id)
    {
        var slip = await dbContext.Slips.AsNoTracking().Include(s => s.Tags).SingleOrDefaultAsync(s => s.Id == id);

        SlipAuthorizeGuardian(slip);

        return slip;
    }

    public async Task<Slip?> GetByLinkNameAsync(string linkName)
    {
        return await dbContext.Slips.AsNoTracking().Include(s => s.Tags).SingleOrDefaultAsync(s => s.FriendlyLinkName == linkName);
    }

    public async Task<Slip> CreateAsync(Slip entity)
    {
        var tags = MarkdownHelper.ExtractTags(entity.Content);

        if (tags.Length > 0)
        {
            entity.Tags = [.. tags.Select(t => new SlipTag
            {
                Tag = t
            })];
        }

        await dbContext.Slips.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Slip> UpdateAsync(Slip entity)
    {
        var tags = MarkdownHelper.ExtractTags(entity.Content);

        var existingTagsQuery = dbContext.SlipTags.Where(st => st.SlipId == entity.Id);
        if (tags.Length == 0)
        {
            await existingTagsQuery.ExecuteDeleteAsync();
        }
        else
        {
            var existingTags = await existingTagsQuery.ToArrayAsync();

            var needRemoveTags = existingTags.ExceptBy(tags, ext => ext.Tag);
            dbContext.SlipTags.RemoveRange(needRemoveTags);

            var needInsertTags = tags.Except(existingTags.Select(ext => ext.Tag));
            dbContext.SlipTags.AddRange(needInsertTags.Select(t => new SlipTag
            {
                SlipId = entity.Id,
                Tag = t
            }));
        }

        dbContext.Slips.Update(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<int> DeleteAsync(int id)
        => await dbContext.Slips.Where(s => s.Id == id).ExecuteDeleteAsync();

    public async Task<long[]> GetCreatedTimeWithSlipAsync(long startTime, long endTime)
        => await dbContext.Slips
            .AsNoTracking()
            .Where(s => s.CreatedAt >= startTime)
            .Where(s => s.CreatedAt < endTime)
            .Select(s => s.CreatedAt)
            .ToArrayAsync();

    public async Task<string[]> GetAllTagsAsync()
        => await dbContext.SlipTags
            .AsNoTracking()
            .Where(st => st.Slip.Status == (int)SlipStatus.Public)
            .Select(st => st.Tag)
            .Distinct()
            .ToArrayAsync();

    public bool IsAuthenticated => httpContext.HttpContext.User.Identity?.IsAuthenticated ?? false;

    private void SlipAuthorizeGuardian(Slip? slip)
    {
        if (slip != null && IsNeedAuthorizeSlipStatus(slip.Status) && !IsAuthenticated)
        {
            throw new UnauthorizedAccessException($"Slip status: {(SlipStatus)slip.Status}.");
        }
    }

    private static IEnumerable<int> GetNeedAuthorizeSlipStatus(IEnumerable<int> slipStatus)
        => slipStatus.Except([(int)SlipStatus.Public]);

    private static bool IsNeedAuthorizeSlipStatus(int slipStatus)
        => GetNeedAuthorizeSlipStatus([slipStatus]).Any();
}
