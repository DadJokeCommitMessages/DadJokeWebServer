using DadJokeAPI.Middleware;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;

namespace DadJokeAPI.Converters;

public class UsersConverter
{
    private readonly IJokesRepository _jokesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IJokeTypesRepository _jokeTypesRepository;

    public UsersConverter(IJokesRepository jokesRepository, IUsersRepository usersRepository, IJokeTypesRepository jokeTypesRepository)
    {
        _jokesRepository = jokesRepository;
        _usersRepository = usersRepository;
        _jokeTypesRepository = jokeTypesRepository;
    }

    // TODO: Get logged in user for this
    public Result<User> Convert()
    {
        // convert
        string userEmail = "test@gmail.com";
        
        Result<User> loggedInUserResult =_usersRepository.GetUserByEmail(userEmail);
        
        if (loggedInUserResult.IsFailure)
            return Result.Fail<User>(loggedInUserResult.ValidationErrors);

        return Result.Ok(loggedInUserResult.Value);
    }

    public Result<User> Convert(GoogleResponse googleUser)
    {
        User user = new User
        {
            AuthorName = googleUser.Name,
            EmailAddress = googleUser.Email
        };
        
        return Result.Ok(user);
    }
    
}