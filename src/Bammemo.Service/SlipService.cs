using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Helpers;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Models.Slips;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Bammemo.Service.Extensions;

namespace Bammemo.Service;

public class SlipService(
    BammemoDbContext dbContext,
    IHttpContextAccessor httpContext,
    IStorageService storageService) : ISlipService
{
    public async Task<Slip[]> ListAsync(
        ListSlipQueryRequestDto? query,
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

    public async Task<bool> CheckLinkNameExistsAsync(int currentSlipId, string linkName)
    {
        return await dbContext.Slips.AsNoTracking().AnyAsync(s => s.Id != currentSlipId && s.FriendlyLinkName == linkName);
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

        entity.CreatedAt = DateTime.UtcNow.Ticks;

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

        entity.UpdateAt = DateTime.UtcNow.Ticks;

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

    public async Task AddAttachmentsAsync(int slipId, IEnumerable<AddSlipAttachmentInfo> attachmentInfos)
    {
        var newFileReferences = new List<FileReference>();
        var needUpdateFileReferences = new List<FileReference>();

        var fileMetadatas = (await storageService.GetFileMetadatasBySourceIdAsync(slipId)).ToDictionary(fm => fm.Id, fm => fm);

        foreach (var attachment in attachmentInfos)
        {
            if (fileMetadatas.TryGetValue(attachment.FileMetadataId, out var fileMetadata))
            {
                var reference = fileMetadata.References?.SingleOrDefault(reference => reference.SourceId == slipId);
                if(reference != null && reference.ShowThumbnail != attachment.ShowThumbnail)
                {
                    reference.ShowThumbnail = attachment.ShowThumbnail;
                    needUpdateFileReferences.Add(reference);
                }
            }
            else
            {
                newFileReferences.Add(new FileReference
                {
                    MetadataId = attachment.FileMetadataId,
                    SourceId = slipId,
                    SourceType = (int)FileReferenceSourceType.Slip,
                    ShowThumbnail = attachment.ShowThumbnail
                });
            }
        }

        await storageService.SaveReferencesAsync(newFileReferences, needUpdateFileReferences);
    }

    public async Task<SlipAttachmentDto[]> LoadAttachmentsAsync(int slipId)
    {
        var attachments = await LoadAttachmentsAsync([slipId]);
        if (attachments.Count == 1)
        {
            return attachments.First().Value;
        }

        return [];
    }

    public async Task<Dictionary<int, SlipAttachmentDto[]>> LoadAttachmentsAsync(IEnumerable<int> slipIds)
    {
        var filesQuery = from m in dbContext.FileMetadata
                         join fr in dbContext.FileReferences on m.Id equals fr.MetadataId
                         where fr.SourceType == (int)FileReferenceSourceType.Slip
                         where slipIds.Contains(fr.SourceId)
                         select new
                         {
                             FileMetadata = m,
                             fr.SourceId,
                             fr.ShowThumbnail
                         };

        var filesList = await filesQuery.AsNoTracking().ToListAsync();

        return filesList.GroupBy(f => f.SourceId, f => new SlipAttachmentDto
        {
            FileMetadataId = f.FileMetadata.Id,
            FileName = f.FileMetadata.FileName,
            Url = f.FileMetadata.GetUrl(httpContext.HttpContext.Request).ToString(),
            ShowThumbnail = f.ShowThumbnail ?? false
        }).ToDictionary(g => g.Key, g => g.ToArray());
    }

    private bool IsAuthenticated => httpContext.HttpContext.User.Identity?.IsAuthenticated ?? false;

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
