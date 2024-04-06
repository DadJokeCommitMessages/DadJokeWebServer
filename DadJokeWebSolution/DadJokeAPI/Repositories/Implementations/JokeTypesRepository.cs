using DadJokeAPI.Data;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace DadJokeAPI.Repositories.Implementations;

public class JokeTypesRepository : IJokeTypesRepository
{
    private readonly ApplicationDbContext _dbContext;

    public JokeTypesRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Result<JokeType> GetJokeTypeByDescription(string jokeType)
    {
        var dbJokeType = _dbContext
            .JokeType
            .FirstOrDefault(type => type.Description == jokeType.ToLower());

        if (dbJokeType is null)
            return Result.Fail<JokeType>
                (new ValidationError("jokeType", $"JokeType '{jokeType}' Was Not Found."));

        return Result.Ok(dbJokeType);
    }

    public Result<IEnumerable<JokeType>> GetAllJokeTypes()
    {
        IEnumerable<JokeType> jokeTypes = _dbContext
            .JokeType
            .ToList();

        return Result.Ok(jokeTypes);
    }
}