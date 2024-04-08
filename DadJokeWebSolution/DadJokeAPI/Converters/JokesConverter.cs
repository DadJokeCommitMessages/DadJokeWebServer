using DadJokeAPI.Models.Domain;
using DadJokeAPI.Models.DTO;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;

namespace DadJokeAPI.Converters;

public class JokesConverter
{
    
    private readonly IJokesRepository _jokesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IJokeTypesRepository _jokeTypesRepository;

    public JokesConverter(IJokesRepository jokesRepository, IUsersRepository usersRepository, IJokeTypesRepository jokeTypesRepository)
    {
        _jokesRepository = jokesRepository;
        _usersRepository = usersRepository;
        _jokeTypesRepository = jokeTypesRepository;
    }
    

    public Result<Joke> Convert(CreateJokeRequest dto, User loggedInUser)
    {
        Result<User> loggedInUserResult = _usersRepository.GetUserByEmail(loggedInUser.EmailAddress);

        if (loggedInUserResult.IsFailure)
            return Result.Fail<Joke>(loggedInUserResult.ValidationErrors);

        Result<JokeType> existingJokeTypeResult = _jokeTypesRepository.GetJokeTypeByDescription(dto.JokeType);
        
        if (existingJokeTypeResult.IsFailure)
            return Result.Fail<Joke>(existingJokeTypeResult.ValidationErrors);
        
        var result = new Joke
        {
            Story = dto.Story,
            User = loggedInUserResult.Value,
            JokeType = existingJokeTypeResult.Value
        };

        return Result.Ok(result);
    }

    public Result<Joke> Convert(int jokeId, UpdateJokeRequest dto, User loggedInUser)
    {
        Result<JokeType> existingJokeTypeResult = _jokeTypesRepository.GetJokeTypeByDescription(dto.JokeType);
        
        if (existingJokeTypeResult.IsFailure)
            return Result.Fail<Joke>(existingJokeTypeResult.ValidationErrors);
        
        Result<User> loggedInUserResult = _usersRepository.GetUserByEmail(loggedInUser.EmailAddress);
        
        if (loggedInUserResult.IsFailure)
            return Result.Fail<Joke>(loggedInUserResult.ValidationErrors);
        
        Joke newJoke = new Joke
        {
            JokeID = jokeId,
            Story = dto.Story,
            User = loggedInUserResult.Value,
            JokeType = existingJokeTypeResult.Value
        };

        return Result.Ok(newJoke);
    }
    
}