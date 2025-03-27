using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Web.Client.CommonServices;
using Bammemo.Web.Client.Options;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.Kiota.Http.HttpClientLibrary;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var bammemoOptions = await AddBammemoWebClientOptionsAsync(builder);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services.AddFluentUIComponents();

builder.Services.AddScoped(_ =>
{
    var httpClient = KiotaClientFactory.Create(finalHandler: new HttpClientHandler { AllowAutoRedirect = false });
    var adapter = new HttpClientRequestAdapter(new Microsoft.Kiota.Abstractions.Authentication.AnonymousAuthenticationProvider(), httpClient: httpClient)
    {
        BaseUrl = new Uri(bammemoOptions.ApiUrl).GetLeftPart(UriPartial.Authority)
    };
    var client = new Bammemo.Web.Client.WebApis.Client.WebApiClient(adapter);

    return client;
});

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