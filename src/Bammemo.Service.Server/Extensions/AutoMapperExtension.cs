using AutoMapper;
using Bammemo.Service.Server.Attributes;
using Bammemo.Service.Server.Interfaces;
using System.Reflection;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // 命名空间与文件夹结构不匹配

public static class AutoMapperExtension
{
    public static IServiceCollection AddBammemoAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddSingleton(provider => new MapperConfiguration(cfg =>
            {
                var scope = provider.CreateScope();
                var idService = scope.ServiceProvider.GetRequiredService<IIdService>();

                foreach (var assembly in assemblies)
                {
                    foreach (var mapperProfileType in assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(Profile))))
                    {
                        if (mapperProfileType.GetCustomAttribute<NeedIdServiceAttribute>() != null)
                        {
                            cfg.AddProfile(Activator.CreateInstance(mapperProfileType, idService) as Profile);
                        }
                        else
                        {
                            cfg.AddProfile(mapperProfileType);
                        }
                    }
                }
            }).CreateMapper());
}
