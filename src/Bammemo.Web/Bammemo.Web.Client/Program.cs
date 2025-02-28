using Bammemo.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddFluentUIComponents();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpClient<WebApiClient>(client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? throw new NullReferenceException("Please config ApiUrl first")));

builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ICommonSettingService, CommonSettingService>();

await builder.Build().RunAsync();
