using Bammemo.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service;

public class ClientSlipService : ISlipService
{
    public Task<Slip> CreateAsync(Slip slip)
    {
        throw new NotImplementedException();
    }
}
