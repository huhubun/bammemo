using Bammemo.Data;
using Bammemo.Service;
using Bammemo.Service.Identities;
using Bammemo.Service.Interfaces;
using Bammemo.Service.MapperProfiles;
using Bammemo.Service.Options;
using Bammemo.Web.Components;
using Bammemo.Web.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var bammemoOptions = builder.Configuration.GetSection(BammemoOptions.Position).Get<BammemoOptions>() ?? throw new NullReferenceException(nameof(BammemoOptions));

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAntiforgery(options => options.Cookie.Name = ".bammemo.anti");
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddIdentity<BammemoUser, BammemoRole>()
    .AddUserManager<BammemoUserManager>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".bammemo.identity";
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
});
builder.Services.AddTransient<IUserStore<BammemoUser>, BammemoUserStore>();
builder.Services.AddTransient<IRoleStore<BammemoRole>, BammemoRoleStore>();

builder.Services.AddMemoryCache();
builder.Services.AddFluentUIComponents();

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
    typeof(SlipProfile).Assembly);

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
