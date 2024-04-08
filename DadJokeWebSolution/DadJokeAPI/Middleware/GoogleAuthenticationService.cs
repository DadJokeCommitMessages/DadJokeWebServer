using System.Text.Json;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Results;

namespace DadJokeAPI.Middleware;

public class GoogleAuthenticationService
{
    private readonly HttpClient _httpClient = new HttpClient();

    public async Task<Result<GoogleResponse>> ValidateAndExtractUser(string accessToken)
    {
        string url = $"https://www.googleapis.com/oauth2/v1/userinfo?alt=json&access_token={accessToken}";
        
        HttpResponseMessage response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            return Result.Fail<GoogleResponse>(new ValidationError("Could Not Authenticate Client With Google."));
        
        string userDetails = await response.Content.ReadAsStringAsync();
        
        var user = JsonSerializer.Deserialize<GoogleResponse>(userDetails);
        
        // check inside as well.
        if (user is null)
            return Result.Fail<GoogleResponse>(new ValidationError("Could Not Authenticate Client With Google."));

        return Result.Ok(user);
    }
}