using DadJokeAPI.Models.Domain;

namespace DadJokeAPI.Repositories.Interfaces;

public interface IJokesRepository
{
    Task<Joke> CreateJoke(Joke joke);
    
    Task<Joke?> GetRandomJoke(string jokeType);

    Task<Joke?> GetJokeById(int jokeId);
    
    Task<Joke?> UpdateJoke(Joke newJoke);
    
    Task<Joke?> DeleteJokeById(int jokeId);
}