using DadJokeAPI.Data;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace DadJokeAPI.Repositories.Implementations;

public class JokesRepository : IJokesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JokesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Result<Joke> CreateJoke(Joke joke)
    {
        _dbContext.Joke.Add(joke);
        
        _dbContext.SaveChanges();
        
        return Result.Ok(joke);
    }

    public Result<Joke> GetRandomJoke(string jokeType)
    {
        IQueryable<Joke> query = _dbContext.Joke;

        if (jokeType != "random")
        {
            var existing = _dbContext.JokeType.Any(dbJokeType => dbJokeType.Description == jokeType);
            if (!existing)
                return Result.Fail<Joke>
                    (new ValidationError("jokeType", $"JokeType '{jokeType}' Was Not Found."));

            query = query.Where(j => j.JokeType.Description == jokeType);
        }
        
        var result = query
            .Skip(new Random().Next(0, query.Count()))
            .Include(joke => joke.JokeType)
            .FirstOrDefault();
        
        if (result is null) 
            return Result.Fail<Joke>
                (new ValidationError("jokeType",   $"No Jokes Found For JokeType '{jokeType}'."));

        return Result.Ok(result);
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

    public Result<Joke> UpdateJoke(Joke newJoke)
    {
        var existingJoke = _dbContext
            .Joke
            .Include(existing => existing.JokeType)
            .Include(existing => existing.User)
            .FirstOrDefault(existing => existing.JokeID == newJoke.JokeID);

        if (existingJoke is null)
            return Result.Fail<Joke>(new ValidationError("JokeId", "Joke For JokeId Does Not Exist."));

        _dbContext.Entry(existingJoke).CurrentValues.SetValues(newJoke);
        
        existingJoke.JokeType = newJoke.JokeType;
        
        existingJoke.User = newJoke.User;
        
        _dbContext.SaveChanges();
        
        return Result.Ok(newJoke);
    }

    public Result<Joke> DeleteJokeById(int jokeId)
    {
        var existingJoke = _dbContext
            .Joke
            .Include(existing => existing.JokeType)
            .FirstOrDefault(existing => existing.JokeID == jokeId);

        if (existingJoke is null)
            return Result.Fail<Joke>(new ValidationError("JokeId", "Joke For JokeId Does Not Exist."));

        _dbContext.Joke.Remove(existingJoke);
        
        _dbContext.SaveChanges();
        
        return Result.Ok(existingJoke);
    }
}