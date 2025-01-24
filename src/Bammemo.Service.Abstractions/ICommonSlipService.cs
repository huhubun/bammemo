using Bammemo.Service.Abstractions.Paginations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Abstractions;

public interface ICommonSlipService
{
    Task<Dtos.ListSlipDto[]> ListAsync(CursorPagingRequest<string>? paging);
}
