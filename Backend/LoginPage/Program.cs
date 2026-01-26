using LoginPage.Application.Interfaces;
using LoginPage.Infrastructure.AbuseChecker;
using LoginPage.Infrastructure.BloomFilter;
using LoginPage.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using LoginPage.Infrastructure.FileService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSingleton<IBloomFilterService, InMemoryBloomFilter>();
builder.Services.AddSingleton<IAbuseCheckerService, AbuseWorkChecker>();

builder.Services.AddDbContext<PostgresDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DBConnectionString")));

builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<IBloomFilterService>();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
