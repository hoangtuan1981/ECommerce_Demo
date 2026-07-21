
namespace Product.Application.Common.Results;

public sealed record Error(string Code,
    string Message,
    ErrorType Type = ErrorType.Failure)
{
    public static readonly Error None = new(
    string.Empty,
    string.Empty,
    ErrorType.Failure);

    public bool IsNone => this == None;
    public static Error Failure(string code, string message)
        => new(code, message, ErrorType.Failure);

    public static Error Validation(string code, string message)
        => new(code, message, ErrorType.Validation);

    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);

}
