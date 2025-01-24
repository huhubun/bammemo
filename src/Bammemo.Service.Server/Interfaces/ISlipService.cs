using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Server.Interfaces;

public interface ISlipService
{
    Task<Slip[]> ListAsync(CursorPagingRequest<int>? paging);
    //Task<Slip?> GetByIdAsync(int id);
    Task<Slip> CreateAsync(Slip slip);
}
