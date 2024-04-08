using System.Text.Json.Serialization;

namespace DadJokeAPI.Middleware;

[Serializable]
public class GoogleResponse
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
}