using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;
using Microsoft.AspNetCore.Mvc;

namespace DadJokeAPI.Controllers;

[Route("api/jokeType")]
public class JokeTypesController : Controller
{
    private readonly IJokeTypesRepository _jokeTypesRepository;

    public JokeTypesController(IJokeTypesRepository jokeTypesRepository)
    {
        _jokeTypesRepository = jokeTypesRepository;
    }

    [HttpGet]
    [Route("all")]
    public IActionResult GetAllJokeTypes()
    {
        Result<IEnumerable<JokeType>> jokeTypesResult = _jokeTypesRepository.GetAllJokeTypes();

        List<string> descriptions = jokeTypesResult.Value.Select(jokeType => jokeType.Description).ToList();
        
        return Ok(descriptions);
    }
}