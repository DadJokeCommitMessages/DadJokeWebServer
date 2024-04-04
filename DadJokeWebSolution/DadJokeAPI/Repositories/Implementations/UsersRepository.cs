using DadJokeAPI.Data;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DadJokeAPI.Repositories.Implementations;

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UsersRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public Task<User?> GetUserByEmail(string emailAddress)
    {
        return _dbContext.User.FirstOrDefaultAsync(user => user.EmailAddress == emailAddress);
    }

    public async Task<IEnumerable<Joke>> GetAllJokesByUserId(int id)
    {
        return await _dbContext
            .Joke
            .Include(joke => joke.JokeType)
            .Where(joke => joke.User.UserID == id).ToListAsync();
    }

}