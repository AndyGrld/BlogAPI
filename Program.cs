global using Microsoft.EntityFrameworkCore;
global using BlogAPI.Models;
global using BlogAPI.Data;
global using Microsoft.AspNetCore.Mvc;
using BlogAPI.Repository.Interfaces;
using BlogAPI.Repository;
using BlogAPI;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Connect to sqlite database
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=Database.db")
);

// Register repository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
