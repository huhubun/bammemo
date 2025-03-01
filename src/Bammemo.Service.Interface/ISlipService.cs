using Bammemo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Interface;

public interface ISlipService
{
    Task<Slip> CreateAsync(Slip slip);
}
