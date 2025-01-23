using Bammemo.Data;
using Bammemo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service;

public class ServerSlipService(BammemoDbContext dbContext) : ISlipService
{
    public async Task<Slip> CreateAsync(Slip slip)
    {
        await dbContext.Slips.AddAsync(slip);
        await dbContext.SaveChangesAsync();

        return slip;
    }
}
