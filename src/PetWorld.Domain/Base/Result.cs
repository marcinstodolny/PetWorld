namespace PetWorld.Domain.Base;

public class Result
{
    protected Result(bool isSuccess, IReadOnlyCollection<string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public IReadOnlyCollection<string> Errors { get; }

    public static Result Success() => new(true, Array.Empty<string>());

    public static Result Fail(params string[] errors) => new(false, errors ?? Array.Empty<string>());

    public static Result Fail(IEnumerable<string> errors) => new(false, errors?.ToArray() ?? Array.Empty<string>());

    public static Result<TValue> Success<TValue>(TValue value) => Result<TValue>.Success(value);

    public static Result<TValue> Fail<TValue>(params string[] errors) => Result<TValue>.Fail(errors);

    public static Result<TValue> Fail<TValue>(IEnumerable<string> errors) => Result<TValue>.Fail(errors);
}

public sealed class Result<TValue> : Result
{
    private Result(bool isSuccess, TValue? value, IReadOnlyCollection<string> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public TValue? Value { get; }

    public static Result<TValue> Success(TValue value) => new(true, value, Array.Empty<string>());

    public new static Result<TValue> Fail(params string[] errors) => new(false, default, errors ?? Array.Empty<string>());

    public new static Result<TValue> Fail(IEnumerable<string> errors) => new(false, default, errors?.ToArray() ?? Array.Empty<string>());
}