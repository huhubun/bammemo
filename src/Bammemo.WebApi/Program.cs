using Bammemo.Data;
using Bammemo.Service.Server;
using Bammemo.Service.Server.Helpers;
using Bammemo.Service.Server.Interfaces;
using Bammemo.WebApi.MapperProfiles;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BammemoDbContext>(options =>
    options.UseSqlite("Data Source=/bammemo/bammemo.db")
);

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(typeof(SlipProfile).Assembly);

builder.Services.AddScoped<ISlipService, SlipService>();
builder.Services.AddScoped<ISettingService, SettingService>();
builder.Services.AddSingleton<IIdService, IdService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var settingService = scope.ServiceProvider.GetRequiredService<ISettingService>();
    var setting = await settingService.GetByKeyAsync(SettingKeys.IdAlphabet);

    if(setting == null)
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

app.UseAuthorization();

app.MapControllers();

app.Run();
