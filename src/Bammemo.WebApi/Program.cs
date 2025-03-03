using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Helpers;
using Bammemo.Service.Server.Interfaces;
using Bammemo.Service.Server.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var bammemoOptions = builder.Configuration.GetSection(BammemoOptions.Position).Get<BammemoOptions>() ?? throw new NullReferenceException(nameof(BammemoOptions));

builder.Services.AddCors(
    options => options.AddDefaultPolicy(
        policy =>
        {
            var origins = new List<string?> { bammemoOptions.ApiUrlAuthority, bammemoOptions.WebUrlAuthority };

            policy.WithOrigins([.. origins.Where(o => !String.IsNullOrEmpty(o)).Distinct().Cast<string>()])
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
    var connectionString = new SqliteConnectionStringBuilder(bammemoOptions.ConnectionString);

    if (!File.Exists(connectionString.DataSource))
    {
        var directory = Path.GetDirectoryName(connectionString.DataSource);
        if (!String.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

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
