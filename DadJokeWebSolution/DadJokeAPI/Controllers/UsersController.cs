using DadJokeAPI.Models.DTO;
using DadJokeAPI.Repositories.Interfaces;
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
    [Route("{userEmail}/jokes")]
    public async Task<IActionResult> GetJokesByUser([FromRoute] string userEmail)
    {
        var existingUser = await _usersRepository.GetUserByEmail(userEmail);
        if (existingUser is null)
            return NotFound("User does not exist.");
        
        // get all the jokes that belong to the person
        var userJokes = await _usersRepository.GetAllJokesByUserId(existingUser.UserID);

        var result = userJokes
            .Select(joke => new JokeDTO 
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