using AutoMapper;
using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Interfaces;
using Bammemo.Web.Client.Layout;
using Bammemo.Web.Components;
using Bammemo.Web.Components.Layout;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<BammemoDbContext>(options =>
    options.UseSqlite("Data Source=/bammemo/bammemo.db")
);

builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });

builder.Services.AddAutoMapper(
    typeof(Program).Assembly, 
    typeof(Bammemo.Service.Server.MapperProfiles.SlipProfile).Assembly);
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    var scope = provider.CreateScope();
    cfg.AddProfile(new Bammemo.Service.Server.MapperProfiles.SlipProfile(scope.ServiceProvider.GetRequiredService<IIdService>()));
}).CreateMapper());

builder.Services.AddScoped<IAppVersionService, AppVersionService>();
builder.Services.AddScoped<CacheStorageAccessor>();
builder.Services.AddHttpClient<IStaticAssetService, ServerStaticAssetService>();

builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<IIdService, IdService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Bammemo.Web.Client._Imports).Assembly);

app.Run();
