using AutoMapper;
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

builder.Services.AddAutoMapper(
    typeof(Program).Assembly,
    typeof(Bammemo.Service.Server.MapperProfiles.SlipProfile).Assembly);
builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    var scope = provider.CreateScope();
    cfg.AddProfile(new Bammemo.Service.Server.MapperProfiles.SlipProfile(scope.ServiceProvider.GetRequiredService<IIdService>()));
}).CreateMapper());

builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddScoped<IIdService, IdService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();
    var setting = await settingService.GetByKeyAsync(SettingKeys.IdAlphabet);

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
