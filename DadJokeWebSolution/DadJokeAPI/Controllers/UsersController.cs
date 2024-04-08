using DadJokeAPI.Converters;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Models.DTO;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace DadJokeAPI.Controllers;

[Route("api/user")]
public class UsersController : Controller
{
    private readonly IUsersRepository _usersRepository;

    public UsersController(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    [HttpGet]
    [Route("jokes")]
    public IActionResult GetJokesByUser()
    {
        Result<User> loggedInUserResult = GetLoggedInUser();
        
        if (loggedInUserResult.IsFailure) 
            return NotFound(loggedInUserResult.ValidationErrors);
        
        Result<IEnumerable<Joke>> userJokesResult = _usersRepository.GetAllJokesByUserId(loggedInUserResult.Value.UserID);

        if (userJokesResult.IsFailure)
            return NotFound(userJokesResult.ValidationErrors);
        
        IEnumerable<JokeResponse> result = userJokesResult
            .Value
            .Select(joke => new JokeResponse 
                {
                    JokeID = joke.JokeID,
                    Story = joke.Story,
                    JokeType = joke.JokeType.Description
                }
            )
            .ToList();

        return Ok(result);
    }
    
    private Result<User> GetLoggedInUser()
    {
        var loggedInUser = HttpContext.Items["loggedInUser"] as User;

        if (loggedInUser is null)
            Result.Fail<User>(new ValidationError("Could Not Determine Logged In User"));

        return Result.Ok(loggedInUser);
    }

}