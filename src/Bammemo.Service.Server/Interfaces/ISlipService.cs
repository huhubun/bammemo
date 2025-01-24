using Bammemo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Server.Interfaces;

public interface ISlipService
{
    Task<Slip[]> ListAsync();
    //Task<Slip?> GetByIdAsync(int id);
    Task<Slip> CreateAsync(Slip slip);
}
