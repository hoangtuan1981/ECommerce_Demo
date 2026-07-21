namespace Product.Application.Common.Results;

public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    private Result(
        TValue value)
        : base(true, Error.None)
    {
        _value = value;
    }

    private Result(
        Error error)
        : base(false, error)
    {
        _value = default;
    }

    public TValue Value =>
        IsSuccess
            ? _value!
            : throw new InvalidOperationException(
                "Cannot access the value of a failed result.");

    public static Result<TValue> Success(
        TValue value)
        => new(value);

    public new static Result<TValue> Failure(
        Error error)
        => new(error);

    public static implicit operator Result<TValue>(TValue value)
        => Success(value);
}