using Microsoft.EntityFrameworkCore;
using AuctionApi.Services;
using AuctionApi.Domain.Data;
using AuctionApi.Domain.Models;
using Microsoft.Extensions.Caching;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AuctionDbContext>(options => options.UseSqlite(builder.Configuration["WebAPIConnection"]));
builder.Services.AddDistributedRedisCache(options => { options.Configuration = builder.Configuration["RedisConnectionString"]; });
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuctionRepo, AuctionRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
