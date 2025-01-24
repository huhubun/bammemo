using Bammemo.Service.Abstractions;
using Bammemo.Service.Abstractions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Client;

public class SlipService : ISlipService
{
    public IQueryable<Slip> Query()
    {
        throw new NotImplementedException();
    }


    public Task<Slip?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Slip?> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Dtos.Slip> CreateAsync(Dtos.Slip slip)
    {
        throw new NotImplementedException();
    }
}
