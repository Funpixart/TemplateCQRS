namespace TemplateCQRS.Application.Common;

/// <summary>
///     Represents the result of an operation that can either succeed with
///     a value of type TData, or fail with an error of type TError.
/// </summary>
public class Payload<TData, TError>
{
    /// <summary>
    ///     Gets the data of the successful operation, or default(TData) if the operation failed.
    /// </summary>
    public TData Data { get; } = default!;

    /// <summary>
    ///     Gets the error of the failed operation, or default(TError) if the operation succeeded.
    /// </summary>
    public TError Error { get; } = default!;

    /// <summary>
    ///     Gets a value indicating whether the operation succeeded.
    ///     The operation is considered to have succeeded if Error is null.
    /// </summary>
    public bool IsSuccess => Error == null;

    public Payload(TData data) => Data = data;

    public Payload(TError error) => Error = error;

    /// <summary>
    ///     Creates a new Payload representing a successful operation with the given data.
    /// </summary>
    public static Payload<TData, TError> Success(TData value) => new(value);

    /// <summary>
    ///     Creates a new Payload representing a failed operation with the given error.
    /// </summary>
    public static Payload<TData, TError> Failure(TError error) => new(error);

    /// <summary>
    ///     Calls the success function if the operation was successful,
    ///     or the failure function if the operation failed, and returns the result.
    /// </summary>
    public TResult Match<TResult>(Func<TData, TResult> success, Func<TError, TResult> failure) => IsSuccess ? success(Data) : failure(Error);

    public static implicit operator Payload<TData, TError>(TData data) => new(data);
    public static implicit operator Payload<TData, TError>(TError error) => new(error);
}

