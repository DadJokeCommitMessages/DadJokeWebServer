using System.Text.Json;
using DadJokeAPI.Converters;
using DadJokeAPI.Models.Domain;
using DadJokeAPI.Repositories.Interfaces;
using DadJokeAPI.Results;

namespace DadJokeAPI.Middleware;

public class GoogleAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GoogleAuthenticationService _authenticationService = new();

    public GoogleAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUsersRepository usersRepository, UsersConverter usersConverter)
    {
        // must have access token
        string accessToken = context.Request.Headers["Authorization"];
        
        if (string.IsNullOrEmpty(accessToken))
        {
            await BuildErrorResponse(context, StatusCodes.Status401Unauthorized, new ValidationError("Authentication", "User Unauthorized"));
            return;
        }
        
        // must be a google access token with username and email
        Result<GoogleResponse> googleUserResult = await _authenticationService.ValidateAndExtractUser(accessToken);
        
        if (googleUserResult.IsFailure)
        {
            await BuildErrorResponse(context, StatusCodes.Status401Unauthorized, new ValidationError("Authentication", "User Unauthorized"));
            return;
        }
        
        Result<User> loggedInUserResult = usersConverter.Convert(googleUserResult.Value);
        
        if (loggedInUserResult.IsFailure)
        {
            await BuildErrorResponse(context, StatusCodes.Status400BadRequest, loggedInUserResult.ValidationErrors);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        User loggedInUser;
        
        Result<User> dbUserResult = usersRepository.GetUserByEmail(loggedInUserResult.Value.EmailAddress);
        
        if (dbUserResult.IsFailure)
        {
            if (dbUserResult.ValidationErrors.Any(error =>
                    error.ErrorMessage.Contains("Email Does Not Link To An Existing User")))
            {
                // create new user if not registered
                Result<User> savedUserResult = usersRepository.SaveUser(loggedInUserResult.Value);
            
                if (savedUserResult.IsFailure)
                {
                    await BuildErrorResponse(context, StatusCodes.Status500InternalServerError, savedUserResult.ValidationErrors);
                    return;
                }
        
                loggedInUser = savedUserResult.Value;
            }
            else
            {
                await BuildErrorResponse(context, StatusCodes.Status500InternalServerError, dbUserResult.ValidationErrors);
                return;
            }
        }
        else
        {
            loggedInUser = dbUserResult.Value;
        }
        
        context.Items["loggedInUser"] = loggedInUser;
        
        await _next(context);
    }

    private static async Task BuildErrorResponse(HttpContext context, int statusCode, IEnumerable<ValidationError> errors)
    {
        context.Response.StatusCode = statusCode;
        var jsonResponse = JsonSerializer.Serialize(errors);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(jsonResponse);
    }

    private static async Task BuildErrorResponse(HttpContext context, int statusCode, ValidationError error)
    {
        await BuildErrorResponse(context, statusCode, new List<ValidationError> { error });
    }
}
