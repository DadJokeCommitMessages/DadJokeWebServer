using DadJokeAPI.Models.Domain;

namespace DadJokeAPI.Repositories.Interfaces;

public interface IJokeTypesRepository
{
    Task<JokeType?> GetJokeTypeByDescription(string jokeType);
}