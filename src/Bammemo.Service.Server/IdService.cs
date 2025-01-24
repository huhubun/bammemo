using Bammemo.Service.Abstractions;
using Bammemo.Service.Server.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace Bammemo.Service.Server;

public class IdService : IIdService
{
    private readonly Lazy<Task<SqidsEncoder<int>>> sqids;

    public IdService(IServiceProvider serviceProvider)
    {
        sqids = new Lazy<Task<SqidsEncoder<int>>>(async () =>
        {
            var settingService = serviceProvider.GetRequiredService<ISettingService>();
            var idAlphabetSetting = await settingService.GetByKeyAsync(SettingKeys.IdAlphabet);

            ArgumentNullException.ThrowIfNull(idAlphabetSetting);

            return new SqidsEncoder<int>(new SqidsOptions
            {
                Alphabet = idAlphabetSetting.Value,
                MinLength = 6
            });
        });
    }

    public async Task<string> EncodeAsync(int number) => (await sqids.Value).Encode(number);
    public async Task<int> DecodeAsync(string str) => (await sqids.Value).Decode(str).Single();
}
