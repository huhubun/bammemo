using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Configurations;
using Bammemo.Service.Server.Helpers;
using Bammemo.Service.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var bammemoOptions = builder.Configuration.GetSection(BammemoOptions.Position).Get<BammemoOptions>() ?? throw new NullReferenceException(nameof(BammemoOptions));

builder.Services.AddCors(
    options => options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(
            [
                bammemoOptions?.ApiUrl ?? throw new OptionsValidationException(nameof(BammemoOptions.ApiUrl), typeof(BammemoOptions), null),
                bammemoOptions.WebUrl ?? throw new OptionsValidationException(nameof(BammemoOptions.WebUrl), typeof(BammemoOptions), null)
            ])
           .AllowAnyMethod()
           .AllowAnyHeader();
        }));

// Add services to the container.
builder.Services.AddDbContext<BammemoDbContext>(options =>

    options.UseSqlite(bammemoOptions.ConnectionString)
);

builder.Services.Configure<BammemoOptions>(
    builder.Configuration.GetSection(BammemoOptions.Position));

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddBammemoAutoMapper(
    typeof(Program).Assembly,
    typeof(Bammemo.Service.Server.MapperProfiles.SlipProfile).Assembly);

builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IIdService, IdService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    if (!File.Exists("/bammemo/bammemo.db"))
    {
        Directory.CreateDirectory("/bammemo");

        var dbContext = scope.ServiceProvider.GetRequiredService<BammemoDbContext>();
        dbContext.Database.EnsureCreated();
    }

    var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();

    var setting = await settingService.GetByKeyFromCacheAsync(SettingKeys.IdAlphabet);
    if (setting == null)
    {
        var idAlphabet = IdHelper.GenerateIdAlphabet();
        await settingService.CreateAsync(SettingKeys.IdAlphabet, idAlphabet);
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
