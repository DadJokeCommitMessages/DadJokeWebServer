
namespace DadJokeAPI.Results;

public class Result
{
    public bool Success { get; }
    
    public IEnumerable<ValidationError> ValidationErrors { get; private set; }
    
    public bool IsSuccess => Success;
    public bool IsFailure => !Success;
    
    protected Result(bool success, IEnumerable<ValidationError> validationErrors)
    {
        Success = success;
        ValidationErrors = validationErrors;
    }
    
    public static Result Ok()
    {
        return new Result(true, []);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, []);
    }

    public static Result Fail(IEnumerable<ValidationError> validationErrors)
    {
        return new Result(false, validationErrors);
    }

    public static Result<T> Fail<T>(IEnumerable<ValidationError> validationErrors)
    {
        return new Result<T>(default, false, validationErrors);
    }
    
    public static Result<T> Fail<T>(ValidationError validationError)
    {
        return new Result<T>(default, false, new List<ValidationError> { validationError });
    }
}

public class Result<T> : Result
{
    protected internal Result(T value, bool success, IEnumerable<ValidationError> validationErrors)
        : base(success, validationErrors)
    {
        Value = value;
    }

    public T Value { get; set; }
}