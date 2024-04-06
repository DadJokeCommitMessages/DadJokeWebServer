using System.ComponentModel.DataAnnotations;

namespace DadJokeAPI.Models.DTO;

public class UpdateJokeRequest
{
    [StringLength(maximumLength: 255, MinimumLength = 10)]
    public string Story { get; set; }
    
    [StringLength(maximumLength: 255, MinimumLength = 3)]
    public string JokeType { get; set; }
}
