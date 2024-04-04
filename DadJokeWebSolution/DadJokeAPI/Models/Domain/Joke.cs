using System.ComponentModel.DataAnnotations;

namespace DadJokeAPI.Models.Domain;

public class Joke
{
    public int JokeID { get; set; }
    
    public string Story { get; set; }
    
    public User User { get; set; }
    
    public JokeType JokeType { get; set; }
}