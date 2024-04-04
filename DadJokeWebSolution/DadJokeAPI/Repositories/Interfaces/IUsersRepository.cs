using DadJokeAPI.Models.Domain;

namespace DadJokeAPI.Repositories.Interfaces;

public interface IUsersRepository
{

    Task<User?> GetUserByEmail(string emailAddress);

    Task<IEnumerable<Joke>> GetAllJokesByUserId(int id);

}