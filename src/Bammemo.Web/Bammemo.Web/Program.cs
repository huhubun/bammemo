using Bammemo.Data;
using Bammemo.Service;
using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Helpers;
using Bammemo.Service.Identities;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Options;
using Bammemo.Web.Client.Options;
using Bammemo.Web.CommonServices;
using Bammemo.Web.Components;
using Bammemo.Web.Identities;
using Bammemo.Web.MinimalApis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.FluentUI.AspNetCore.Components;
#if DEBUG
using Scalar.AspNetCore;
#endif

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Web
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(option => { option.DetailedErrors = true; });

builder.Services.AddFluentUIComponents();

builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = null;
});

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAntiforgery(options => options.Cookie.Name = ".bammemo.anti");

// Api
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Identity
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddIdentity<BammemoUser, BammemoRole>()
    .AddUserManager<BammemoUserManager>()
    .AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/logout";
    options.Cookie.Name = ".bammemo.identity";
    options.Events = new CookieAuthenticationEvents
    {
        OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                context.Response.Redirect(context.RedirectUri);
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddTransient<IUserStore<BammemoUser>, BammemoUserStore>();
builder.Services.AddTransient<IRoleStore<BammemoRole>, BammemoRoleStore>();

// Common
var bammemoOptions = builder.Configuration.GetSection(BammemoOptions.Position).Get<BammemoOptions>() ?? throw new NullReferenceException(nameof(BammemoOptions));

builder.Services.AddMemoryCache();
builder.Services.AddDbContext<BammemoDbContext>(options =>
    options.UseSqlite(bammemoOptions.ConnectionString)
);

builder.Services.AddOptions<BammemoOptions>()
    .Bind(builder.Configuration.GetSection(BammemoOptions.Position))
    .ValidateDataAnnotations()
    .ValidateOnStart();

builder.Services.AddBammemoStorageProvider();

// 预加载需要
builder.Services.AddSingleton(_ =>
{
    var adapter = new Microsoft.Kiota.Http.HttpClientLibrary.HttpClientRequestAdapter(new Microsoft.Kiota.Abstractions.Authentication.AnonymousAuthenticationProvider())
    {
        BaseUrl = new Uri(bammemoOptions.ApiUrl).GetLeftPart(UriPartial.Authority)
    };
    var client = new Bammemo.Web.Client.WebApis.Client.WebApiClient(adapter);

    return client;
});

builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<IIdService, IdService>();
builder.Services.AddScoped<IRedirectRuleService, RedirectRuleService>();
builder.Services.AddScoped<ISiteLinkService, SiteLinkService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IStorageService, StorageService>();

builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ICommonSettingService, CommonSettingService>();
builder.Services.AddScoped<ICommonSiteLinkService, CommonSiteLinkService>();
builder.Services.AddScoped<ICommonAnalyticsService, CommonAnalyticsService>();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

#if DEBUG
app.UseWebAssemblyDebugging();
app.MapOpenApi();
app.MapScalarApiReference();
#else
app.UseExceptionHandler("/Error", createScopeForErrors: true);
#endif

using (var scope = app.Services.CreateScope())
{
    var connectionString = new SqliteConnectionStringBuilder(bammemoOptions.ConnectionString);

    if (!File.Exists(connectionString.DataSource))
    {
        if (bammemoOptions.Key == null)
        {
            var securityService = scope.ServiceProvider.GetRequiredService<ISecurityService>();
            securityService.GenerateAesKeyToLocalStorage();
        }

        // Init database
        var directory = Path.GetDirectoryName(connectionString.DataSource);
        if (!String.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var dbContext = scope.ServiceProvider.GetRequiredService<BammemoDbContext>();
        await dbContext.Database.MigrateAsync();

        var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();

        var idAlphabet = IdHelper.GenerateIdAlphabet();
        await settingService.CreateAsync(SettingKeys.IdAlphabet, idAlphabet);

        await settingService.CreateAsync(SettingKeys.SiteName, "bammemo");
    }
    else
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BammemoDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}

app.UseAntiforgery();
app.MapStaticAssets();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStatusCodePagesWithRedirects("/404");
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Bammemo.Web.Client._Imports).Assembly);

app.MapGet("/logout", async ([FromServices] SignInManager<BammemoUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return TypedResults.LocalRedirect("~/");
}).ExcludeFromDescription();

app.MapGet("/bammemo.json", async (HttpContext httpContext) =>
{
    var bammemoOptions = httpContext.RequestServices.GetRequiredService<IOptions<BammemoOptions>>();

    await httpContext.Response.WriteAsJsonAsync(new
    {
        Bammemo = new BammemoWebClientOptions
        {
            ApiUrl = bammemoOptions.Value.ApiUrl
        }
    });
}).ExcludeFromDescription();

app.MapJsResourceApi()
    .MapFilesApi();

app.Run();
