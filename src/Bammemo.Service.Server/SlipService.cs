using AutoMapper;
using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Server;

public class SlipService(BammemoDbContext dbContext, IMapper mapper) : ISlipService
{
    public async Task<Slip[]> ListAsync(CursorPagingRequest<int>? paging)
    {
        IQueryable<Slip> slips = dbContext.Slips.OrderByDescending(s => s.Id);
        var take = paging?.Take ?? 5;

        if (paging != null)
        {
            slips = slips.Where(s => s.Id < paging.Cursor);
        }

        return await slips.Take(take).ToArrayAsync();

    }

    //public async Task<Dtos.SlipDto?> GetByIdAsync(int id)
    //{
    //    var entity = await dbContext.Slips.SingleOrDefaultAsync(s => s.Id == id);
    //    return entity == null ? null : mapper.Map<Dtos.SlipDto>(entity);
    //}

    //public async Task<Dtos.SlipDto?> GetByIdAsync(string id)
    //{
    //    var rawId = 0;
    //    return await GetByIdAsync(rawId);
    //}

    public async Task<Slip> CreateAsync(Slip entity)
    {
        // TODO check url, add tags...

        await dbContext.Slips.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        return entity;
    }

    //public string EncodedId(int id)
    //{
    //}
}
