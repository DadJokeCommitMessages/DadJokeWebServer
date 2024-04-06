using DadJokeAPI.Converters;
using DadJokeAPI.Data;
using DadJokeAPI.Repositories.Implementations;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

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

builder.WebHost.UseUrls("http://*:5282");

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