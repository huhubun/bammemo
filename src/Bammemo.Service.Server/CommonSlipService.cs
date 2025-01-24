using Bammemo.Service.Abstractions;

namespace Bammemo.Service.Server;

public class CommonSlipService : ICommonSlipService
{
    public Task<Dtos.SlipDto[]> ListAsync()
    {
        throw new NotImplementedException();
    }
}
