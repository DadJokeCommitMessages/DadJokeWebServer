using DadJokeAPI.Data;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DadJokeAPI.Repositories.Implementations;

public class JokesRepository : IJokesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JokesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Joke> CreateJoke(Joke joke)
    {
        await _dbContext.Joke.AddAsync(joke);
        await _dbContext.SaveChangesAsync();
        return joke;
    }

    public Task<Joke?> GetRandomJoke(string jokeType)
    {
        IQueryable<Joke> query = _dbContext.Joke;

        if (jokeType != "random")
        {
            query = query.Where(j => j.JokeType.Description == jokeType);
        }
        
        var count = query.Count();
        var randomIndex = new Random().Next(0, count);

        return query
            .Skip(randomIndex)
            .Include(joke => joke.JokeType)
            .FirstOrDefaultAsync();
    }

    public async Task<Joke?> GetJokeById(int jokeId)
    {
        return await _dbContext
            .Joke
            .FirstOrDefaultAsync(joke => joke.JokeID == jokeId);
    }

    public async Task<IEnumerable<Joke>> GetAllJokesByUser(string userEmail)
    {
        return await _dbContext
            .Joke
            .Where(joke => joke.User.EmailAddress.Equals(userEmail))
            .Include(joke => joke.JokeType)
            .ToListAsync();
    }

    public async Task<Joke?> UpdateJoke(Joke newJoke)
    {
        var existingJoke = _dbContext
            .Joke
            .Include(existing => existing.JokeType)
            .Include(existing => existing.User)
            .FirstOrDefault(existing => existing.JokeID == newJoke.JokeID);

        if (existingJoke is not null)
        {
            _dbContext.Entry(existingJoke).CurrentValues.SetValues(newJoke);
            existingJoke.JokeType = newJoke.JokeType;
            existingJoke.User = newJoke.User;
            
            await _dbContext.SaveChangesAsync();
            
            return newJoke;
        }

        return null;
    }

    public async Task<Joke?> DeleteJokeById(int jokeId)
    {
        var existingJoke = await _dbContext
            .Joke
            .Include(existing => existing.JokeType)
            .FirstOrDefaultAsync(existing => existing.JokeID == jokeId);

        if (existingJoke is not null)
        {
            _dbContext.Joke.Remove(existingJoke);
            await _dbContext.SaveChangesAsync();
            return existingJoke;
        }

        return null;
    }
}