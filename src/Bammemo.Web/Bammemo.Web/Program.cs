using Bammemo.Data;
using Bammemo.Service;
using Bammemo.Web.Client.Pages;
using Bammemo.Web.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddFluentUIComponents();

builder.Services.AddDbContext<BammemoDbContext>(options =>
    options.UseSqlite("Data Source=bammemo.db")
);

builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });

builder.Services.AddScoped<ISlipService, ServerSlipService>();

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
