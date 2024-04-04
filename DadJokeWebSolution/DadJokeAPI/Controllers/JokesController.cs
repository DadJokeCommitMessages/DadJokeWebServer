using DadJokeAPI.Models.Domain;
using DadJokeAPI.Models.DTO;
using DadJokeAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DadJokeAPI.Controllers;

[Route("api/joke")]
public class JokesController : Controller
{
    private readonly IJokesRepository _jokesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IJokeTypesRepository _jokeTypesRepository;

    public JokesController(IJokesRepository jokesRepository, IUsersRepository usersRepository, IJokeTypesRepository jokeTypesRepository)
    {
        _jokesRepository = jokesRepository;
        _usersRepository = usersRepository;
        _jokeTypesRepository = jokeTypesRepository;
    }

    // GET /joke with param ?jokeType=random|fix|feature
    [HttpGet]
    public async Task<IActionResult> GetRandomJoke(string jokeType = "random")
    {
        jokeType = jokeType.ToLower();
        if (jokeType != "random" && await _jokeTypesRepository.GetJokeTypeByDescription(jokeType) is null)
            return NotFound("JokeType does not exist.");
        
        var randomJoke = await _jokesRepository.GetRandomJoke(jokeType);
        if (randomJoke is null)
            return NotFound($"No jokes found for JokeType '{jokeType}'.");
        
        JokeDTO result = new JokeDTO
        {
            JokeID = randomJoke.JokeID,
            Story = randomJoke.Story,
            JokeType = randomJoke.JokeType.Description
        };

        return Ok(result);
    }
    
    // POST /joke
    [HttpPost]
    public async Task<IActionResult> CreateJoke([FromBody] CreateJokeRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingJokeType = await _jokeTypesRepository.GetJokeTypeByDescription(request.JokeType);
        if (existingJokeType is null)
            return NotFound("JokeType does not exist.");
    
        // TODO : This will probably change with the introduction of authentication.
        var existingUser = await _usersRepository.GetUserByEmail(request.UserEmail);
        if (existingUser is null)
            return NotFound("User does not exist.");
    
        var entity = new Joke
        {
            Story = request.Story,
            User = existingUser,
            JokeType = existingJokeType
        };
    
        Joke savedJoke = await _jokesRepository.CreateJoke(entity);
        
        JokeDTO result = new JokeDTO
        {
            JokeID = savedJoke.JokeID,
            Story = savedJoke.Story,
            JokeType = savedJoke.JokeType.Description
        };

        return Ok(result);
    }

    // PUT joke/{jokeId}
    [HttpPut]
    [Route("{jokeId:int}")]
    public async Task<IActionResult> UpdateJoke([FromRoute] int jokeId, [FromBody] UpdateJokeRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var existingJokeType = await _jokeTypesRepository.GetJokeTypeByDescription(request.JokeType);
        if (existingJokeType is null)
            return NotFound("JokeType does not exist.");
        
        // TODO : This will probably change with the introduction of authentication.
        var existingUser = await _usersRepository.GetUserByEmail(request.UserEmail);
        if (existingUser is null)
            return NotFound("User does not exist.");


        var newJoke = new Joke
        {
            JokeID = jokeId,
            Story = request.Story,
            User = existingUser,
            JokeType = existingJokeType
        };

        var updatedJoke = await _jokesRepository.UpdateJoke(newJoke);
        
        if (updatedJoke is null)
            return NotFound("The JokeId does not link to an existing joke.");

        JokeDTO result = new JokeDTO
        {
            JokeID = updatedJoke.JokeID,
            Story = updatedJoke.Story,
            JokeType = updatedJoke.JokeType.Description
        };

        return Ok(result);
    }

    // DELETE joke/{jokeId}
    [HttpDelete]
    [Route("{jokeId:int}")]
    public async Task<IActionResult> DeleteJoke([FromRoute] int jokeId)
    {
        var deletedJoke = await _jokesRepository.DeleteJokeById(jokeId);

        if (deletedJoke is null)
            return NotFound("The JokeId does not link to an existing joke.");

        JokeDTO result = new JokeDTO
        {
            JokeID = deletedJoke.JokeID,
            Story = deletedJoke.Story,
            JokeType = deletedJoke.JokeType.Description
        };

        return Ok(result);
    }
}