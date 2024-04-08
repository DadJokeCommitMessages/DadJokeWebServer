using DadJokeAPI.Converters;
using DadJokeAPI.Data;
using DadJokeAPI.Middleware;
using DadJokeAPI.Repositories.Implementations;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Postgres context
var connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");

if (connectionString is null)
{
    Console.WriteLine("Environment Variable 'DATABASE_CONNECTION_STRING' is not set.");
    Environment.Exit(0);
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Add Repositories
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<IJokesRepository, JokesRepository>();

builder.Services.AddScoped<IJokeTypesRepository, JokeTypesRepository>();

builder.Services.AddTransient<JokesConverter>();

builder.Services.AddTransient<UsersConverter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler((_ => { }));

app.UseHttpsRedirection();

app.UseMiddleware<GoogleAuthenticationMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();