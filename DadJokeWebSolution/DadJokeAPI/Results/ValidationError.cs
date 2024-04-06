using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DadJokeAPI.Results;

public class ValidationError
{
    public string Field { get; }
    public string ErrorMessage { get; }

    public ValidationError(string errorMessage)
    {
        Field = "Unknown";
        ErrorMessage = errorMessage;
    }

    public ValidationError(string field, string errorMessage)
    {
        Field = field;
        ErrorMessage = errorMessage;
    }

    public static IEnumerable<ValidationError> ConvertModelState(ModelStateDictionary stateDictionary)
    {
        return stateDictionary
            .Where(pair => pair.Value?.Errors?.Any() ?? false)
            .SelectMany(pair =>
            {
                return pair
                    .Value!
                    .Errors
                    .Select(e => new ValidationError(pair.Key, e.ErrorMessage));
            })
            .ToList();
    }
}