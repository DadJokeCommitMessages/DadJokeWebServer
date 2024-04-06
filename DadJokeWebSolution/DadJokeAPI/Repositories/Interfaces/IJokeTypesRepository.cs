using DadJokeAPI.Models.Domain;
using DadJokeAPI.Results;

namespace DadJokeAPI.Repositories.Interfaces;

public interface IJokeTypesRepository
{
    Result<JokeType> GetJokeTypeByDescription(string jokeType);
    Result<IEnumerable<JokeType>> GetAllJokeTypes();
}