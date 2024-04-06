using DadJokeAPI.Data;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.EntityFrameworkCore;

namespace DadJokeAPI.Repositories.Implementations;

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UsersRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public Result<User> GetUserByEmail(string emailAddress)
    {
        var user = _dbContext
            .User
            .FirstOrDefault(user => user.EmailAddress == emailAddress);

        if (user is null)
            return Result.Fail<User>
                (new ValidationError("Email", "Email Does Not Link To An Existing User."));

        return Result.Ok(user);
    }

    public Result<IEnumerable<Joke>> GetAllJokesByUserId(int userId)
    {
        var user = _dbContext
            .User
            .FirstOrDefault(user => user.UserID == userId);

        if (user is null)
            return Result.Fail<IEnumerable<Joke>>
                (new ValidationError("Email", "Email Does Not Link To An Existing User."));

        IEnumerable<Joke> jokes = _dbContext
            .Joke
            .Include(joke => joke.JokeType)
            .Where(joke => joke.User.UserID == userId)
            .ToList();

        return Result.Ok(jokes);
    }
}