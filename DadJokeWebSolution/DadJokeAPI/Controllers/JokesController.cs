using DadJokeAPI.Converters;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Models.DTO;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace DadJokeAPI.Controllers;

[Route("api/joke")]
public class JokesController : Controller
{
    private readonly IJokesRepository _jokesRepository;
    private readonly JokesConverter _jokesConverter;

    public JokesController(IJokesRepository jokesRepository, JokesConverter jokesConverter)
    {
        _jokesRepository = jokesRepository;
        _jokesConverter = jokesConverter;
    }

    [HttpGet]
    public IActionResult GetRandomJoke(string jokeType = "random")
    {
        Result<Joke> randomJokeResult = _jokesRepository.GetRandomJoke(jokeType);
        
        if (randomJokeResult.IsSuccess)
        {
            Joke randomJoke = randomJokeResult.Value;
            
            JokeResponse result = new JokeResponse
            {
                JokeID = randomJoke.JokeID,
                Story = randomJoke.Story,
                JokeType = randomJoke.JokeType.Description
            };
            
            return Ok(result);
        }

        return NotFound(randomJokeResult.ValidationErrors);
    }
    
    [HttpPost]
    public IActionResult CreateJoke([FromBody] CreateJokeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ValidationError.ConvertModelState(ModelState));
        
        Result<Joke> jokeToSaveResult = _jokesConverter.Convert(request);

        if (jokeToSaveResult.IsFailure)
            return NotFound(jokeToSaveResult.ValidationErrors);
        
        Result<Joke> savedJokeResult = _jokesRepository.CreateJoke(jokeToSaveResult.Value);

        if (savedJokeResult.IsFailure)
            return NotFound(savedJokeResult.ValidationErrors);

        Joke savedJoke = jokeToSaveResult.Value;
        
        JokeResponse result = new JokeResponse
        {
            JokeID = savedJoke.JokeID,
            Story = savedJoke.Story,
            JokeType = savedJoke.JokeType.Description
        };

        return Ok(result);
    }

    [HttpPut]
    [Route("{jokeId:int}")]
    public IActionResult UpdateJoke([FromRoute] int jokeId, [FromBody] UpdateJokeRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ValidationError.ConvertModelState(ModelState));

        Result<Joke> newJokeResult = _jokesConverter.Convert(jokeId, request);

        if (newJokeResult.IsFailure)
            return NotFound(newJokeResult.ValidationErrors);
        
        Result<Joke> updatedJokeResult = _jokesRepository.UpdateJoke(newJokeResult.Value);

        if (updatedJokeResult.IsFailure)
            return NotFound(updatedJokeResult.ValidationErrors);

        Joke updatedJoke = updatedJokeResult.Value;
        
        JokeResponse result = new JokeResponse
        {
            JokeID = updatedJoke.JokeID,
            Story = updatedJoke.Story,
            JokeType = updatedJoke.JokeType.Description
        };

        return Ok(result);
    }

    [HttpDelete]
    [Route("{jokeId:int}")]
    public IActionResult DeleteJoke([FromRoute] int jokeId)
    {
        Result<Joke> deletedJokeResult = _jokesRepository.DeleteJokeById(jokeId);

        if (deletedJokeResult.IsFailure)
            return NotFound(deletedJokeResult.ValidationErrors);

        Joke deletedJoke = deletedJokeResult.Value;
        
        JokeResponse result = new JokeResponse
        {
            JokeID = deletedJoke.JokeID,
            Story = deletedJoke.Story,
            JokeType = deletedJoke.JokeType.Description
        };

        return Ok(result);
    }
}