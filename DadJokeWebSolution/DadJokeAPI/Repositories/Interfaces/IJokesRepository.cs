using DadJokeAPI.Models.Domain;
using DadJokeAPI.Results;

namespace DadJokeAPI.Repositories.Interfaces;

public interface IJokesRepository
{
    Result<Joke> CreateJoke(Joke joke);
    
    Result<Joke> GetRandomJoke(string jokeType);

    Task<Joke?> GetJokeById(int jokeId);
    
    Result<Joke> UpdateJoke(Joke newJoke);
    
    Result<Joke> DeleteJokeById(int jokeId);
}