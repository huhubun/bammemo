using AutoMapper;
using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Server.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Service.Server;

public class SlipService(BammemoDbContext dbContext, IMapper mapper) : ISlipService
{
    public async Task<Slip[]> ListAsync(
        ListSlipQueryRequest? query,
        CursorPagingRequest<int>? paging)
    {
        IQueryable<Slip> slips = dbContext.Slips.OrderByDescending(s => s.Id);

        if (query != null)
        {
            if (query.StartTime.HasValue && query.EndTime.HasValue)
            {
                slips = slips.Where(s => s.CreatedAt >= query.StartTime.Value)
                    .Where(s => s.CreatedAt < query.EndTime.Value);
            }
        }

        var take = paging?.Take ?? 5;

        if (paging != null)
        {
            slips = slips.Where(s => s.Id < paging.Cursor);
        }

        return await slips.Take(take).ToArrayAsync();

    }

    public async Task<Slip?> GetByIdAsync(int id)
    {
        return await dbContext.Slips.SingleOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Slip> CreateAsync(Slip entity)
    {
        // TODO add tags...

        await dbContext.Slips.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<Slip> UpdateAsync(Slip entity)
    {
        // TODO update tags...

        dbContext.Slips.Update(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<long[]> GetCreatedTimeWithSlipAsync(long startTime, long endTime)
        => await dbContext.Slips
            .Where(s => s.CreatedAt >= startTime)
            .Where(s => s.CreatedAt < endTime)
            .Select(s => s.CreatedAt)
            .ToArrayAsync();

}
