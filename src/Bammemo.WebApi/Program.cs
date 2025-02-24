using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Helpers;
using Bammemo.Service.Server.Interfaces;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(
    options => options.AddDefaultPolicy(
        policy => policy.WithOrigins(
            [
                builder.Configuration["ApiUrl"] ?? throw new ArgumentNullException("ApiUrl"),
                builder.Configuration["WebUrl"] ?? throw new ArgumentNullException("WebUrl")
            ])
            .AllowAnyMethod()
            .AllowAnyHeader()));

// Add services to the container.
builder.Services.AddDbContext<BammemoDbContext>(options =>
    options.UseSqlite("Data Source=/bammemo/bammemo.db")
);

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

        var idAlphabet = IdHelper.GenerateIdAlphabet();

        var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();
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
