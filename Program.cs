global using Microsoft.EntityFrameworkCore;
global using BlogAPI.Models;
global using BlogAPI.Data;
global using Microsoft.AspNetCore.Mvc;
using BlogAPI.Repository.Interfaces;
using BlogAPI.Repository;
using BlogAPI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => 
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Add Authentication Scheme
builder.Services.AddAuthentication().AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:Token").Value!
        ))
    };
});

// Connect to sqlite database
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlite("Data Source=Database.db")
);

// Adding cors policy
builder.Services.AddCors( options => options.AddPolicy(
    "corspolicy", policy => {
        policy.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
    }
));
// builder.Services.AddCors(options => options.AddDefaultPolicy(
//     builder => builder.AllowAnyOrigin()
// ));

// Mulitple domain
// Any domain

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

// add cors policy
// app.UseCors();
app.UseCors("corspolicy");

app.UseAuthorization();

// Add use authenticaiton middleware
app.UseAuthentication();

app.MapControllers();

app.Run();
