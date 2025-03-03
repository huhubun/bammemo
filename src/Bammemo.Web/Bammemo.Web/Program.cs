using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Interfaces;
using Bammemo.Service.Server.Options;
using Bammemo.Web.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var bammemoOptions = builder.Configuration.GetSection(BammemoOptions.Position).Get<BammemoOptions>() ?? throw new NullReferenceException(nameof(BammemoOptions));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddFluentUIComponents();
builder.Services.AddMemoryCache();

builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = null;
});

builder.Services.AddDbContext<BammemoDbContext>(options =>
    options.UseSqlite(bammemoOptions.ConnectionString)
);

builder.Services.Configure<BammemoOptions>(
    builder.Configuration.GetSection(BammemoOptions.Position));

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(option => { option.DetailedErrors = true; });

builder.Services.AddBammemoAutoMapper(
    typeof(Program).Assembly,
    typeof(Bammemo.Service.Server.MapperProfiles.SlipProfile).Assembly);

// ‘§≥ œ÷–Ë“™
builder.Services.AddHttpClient<Bammemo.Web.Client.Services.WebApiClient>(client =>
{
    client.BaseAddress = new Uri(bammemoOptions?.ApiUrl ?? throw new OptionsValidationException(nameof(BammemoOptions.ApiUrl), typeof(BammemoOptions), null));
});

builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<IIdService, IdService>();
builder.Services.AddScoped<IRedirectRuleService, RedirectRuleService>();
builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ICommonSettingService, CommonSettingService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();

app.UseStatusCodePagesWithRedirects("/404");

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Bammemo.Web.Client._Imports).Assembly);

app.Run();
