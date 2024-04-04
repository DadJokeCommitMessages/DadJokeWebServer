using DadJokeAPI.Models.Domain;

namespace DadJokeAPI.Data;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<JokeType> JokeType { get; set; }
    public DbSet<Joke> Joke { get; set; }
}