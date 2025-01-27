using Bammemo.Web.Client.Layout;
using Bammemo.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpClient<WebApiClient>(client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? throw new NullReferenceException("Please config ApiUrl first")));

builder.Services.AddSingleton<IAppVersionService, AppVersionService>();
builder.Services.AddSingleton<CacheStorageAccessor>();
builder.Services.AddHttpClient<IStaticAssetService, HttpBasedStaticAssetService>();

builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();


await builder.Build().RunAsync();
