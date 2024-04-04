using System.ComponentModel.DataAnnotations;

namespace DadJokeAPI.Models.DTO;

public class UpdateJokeRequestDTO
{
    [StringLength(maximumLength: 255, MinimumLength = 10)]
    public string Story { get; set; }
    
    [Required]
    [EmailAddress]
    public string UserEmail { get; set; }

    
    [StringLength(maximumLength: 255, MinimumLength = 3)]
    public string JokeType { get; set; }
}
