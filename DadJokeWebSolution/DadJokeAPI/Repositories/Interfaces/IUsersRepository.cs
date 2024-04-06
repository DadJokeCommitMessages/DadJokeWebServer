using DadJokeAPI.Models.Domain;
using DadJokeAPI.Results;

namespace DadJokeAPI.Repositories.Interfaces;

public interface IUsersRepository
{

    Result<User> GetUserByEmail(string emailAddress);

    Result<IEnumerable<Joke>> GetAllJokesByUserId(int userId);

}