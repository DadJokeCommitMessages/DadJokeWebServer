using System.ComponentModel.DataAnnotations;

namespace DadJokeAPI.Models.DTO;

public class CreateJokeRequest
{
    [StringLength(maximumLength: 255, MinimumLength = 10)]
    [Required]
    public string Story { get; set; }
    
    [Required]
    [StringLength(maximumLength: 255, MinimumLength = 3)]
    public string JokeType { get; set; }
}
