using DadJokeAPI.Data;
using DadJokeAPI.Repositories.Implementations;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Postgres context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DadJokeAPIConnectionString"));
});

// Add Repositories
builder.Services.AddScoped<IDadJokesRepository, DadJokesRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

// for CORS - probably going to use a secret or environment variable for the URL to the hosted React
// app.UseCors(options =>
// {
//     options.WithOrigins("http://localhost:4200"); // Allow requests only from http://localhost:4200
//     options.AllowAnyMethod();
//     options.AllowAnyHeader();
// });

app.MapControllers();

app.Run();