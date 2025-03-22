using Bammemo.Service.Storages.Providers;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class BammemoStorageProviderExtension
{
    public static IServiceCollection AddBammemoStorageProvider(this IServiceCollection services)
    {
        services.AddTransient<IStorageProvider, LocalStorageProvider>();
        services.AddTransient<IStorageProvider, TencentCloudCosProvider>();

        return services;
    }
}
