using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Interfaces;
using Bammemo.Web.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

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
    options.UseSqlite("Data Source=/bammemo/bammemo.db")
);

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(option => { option.DetailedErrors = true; });

builder.Services.AddBammemoAutoMapper(
    typeof(Program).Assembly,
    typeof(Bammemo.Service.Server.MapperProfiles.SlipProfile).Assembly);

// ‘§≥ œ÷–Ë“™
builder.Services.AddHttpClient<Bammemo.Web.Client.Services.WebApiClient>(client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"] ?? throw new NullReferenceException("Please config ApiUrl first")));

builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<IIdService, IdService>();
builder.Services.AddScoped<IRedirectRuleService, RedirectRuleService>();
builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ICommonSettingService, CommonSettingService>();

//builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
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
