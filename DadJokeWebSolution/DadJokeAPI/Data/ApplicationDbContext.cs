using DadJokeAPI.Models.Domain;

namespace DadJokeAPI.Data;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<DadJoke> DadJokes { get; set; }
    public DbSet<User> Users { get; set; }
}