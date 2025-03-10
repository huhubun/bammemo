using Bammemo.Web.Client.Extensions;
using Bammemo.Web.Client.Options;
using Bammemo.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var bammemoOptions = await AddBammemoWebClientOptionsAsync(builder);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddFluentUIComponents();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpClient<WebApiClient>(client => client.BaseAddress = new Uri(bammemoOptions.ApiUrl.NormalizeUrlSlash()));

builder.Services.AddScoped<ICommonSlipService, CommonSlipService>();
builder.Services.AddScoped<ICommonSettingService, CommonSettingService>();
builder.Services.AddScoped<ICommonSiteLinkService, CommonSiteLinkService>();
builder.Services.AddScoped<ICommonAnalyticsService, CommonAnalyticsService>();

await builder.Build().RunAsync();

static async Task<BammemoWebClientOptions> AddBammemoWebClientOptionsAsync(WebAssemblyHostBuilder builder)
{
    var httpClient = new HttpClient()
    {
        BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
    };

    using var response = await httpClient.GetAsync(BammemoWebClientOptions.FileName);
    using var stream = await response.Content.ReadAsStreamAsync();

    builder.Configuration.AddJsonStream(stream);

    var section = builder.Configuration.GetSection(BammemoWebClientOptions.Position);
    builder.Services.Configure<BammemoWebClientOptions>(section);

    return section.Get<BammemoWebClientOptions>() ?? throw new NullReferenceException(nameof(BammemoWebClientOptions));
}