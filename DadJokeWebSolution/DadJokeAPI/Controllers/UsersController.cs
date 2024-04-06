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
    private readonly UsersConverter _usersConverter;

    public UsersController(IUsersRepository usersRepository, UsersConverter usersConverter)
    {
        _usersRepository = usersRepository;
        _usersConverter = usersConverter;
    }

    [HttpGet]
    [Route("/jokes")]
    public IActionResult GetJokesByUser()
    {
        Result<User> loggedInUserResult = _usersConverter.Convert();
        
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
}