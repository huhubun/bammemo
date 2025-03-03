using Bammemo.Service.Server.Options;
using Bammemo.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var bammemoOptions = builder.Configuration.GetSection(BammemoWebClientOptions.Position).Get<BammemoWebClientOptions>() ?? throw new NullReferenceException(nameof(BammemoWebClientOptions));

builder.Services.AddFluentUIComponents();

builder.Services.Configure<BammemoWebClientOptions>(
    builder.Configuration.GetSection(BammemoWebClientOptions.Position));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpClient<WebApiClient>(client => client.BaseAddress = new Uri(bammemoOptions.ApiUrl));

builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ICommonSettingService, CommonSettingService>();

await builder.Build().RunAsync();
