using DadJokeAPI.Data;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DadJokeAPI.Repositories.Implementations;

public class JokeTypesRepository : IJokeTypesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JokeTypesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<JokeType?> GetJokeTypeByDescription(string jokeType)
    {
        return _dbContext.JokeType.FirstOrDefaultAsync(type => type.Description == jokeType);
    }
}