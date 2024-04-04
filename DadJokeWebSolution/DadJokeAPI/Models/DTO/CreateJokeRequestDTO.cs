using System.ComponentModel.DataAnnotations;

namespace DadJokeAPI.Models.DTO;

public class CreateJokeRequestDTO
{
    [StringLength(maximumLength: 255, MinimumLength = 10)]
    [Required]
    public string Story { get; set; }
    
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }
    
    [Required]
    [StringLength(maximumLength: 255, MinimumLength = 3)]
    public string JokeType { get; set; }
}
