using System.Diagnostics.CodeAnalysis;

namespace TrueCode.UserService;

public struct TryGetResult<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess { get; private set; }
    
    public T? Value { get; private set; }

    public static TryGetResult<T> Success(T value)
    {
        return new TryGetResult<T> { Value = value, IsSuccess = true };
    }

    public static TryGetResult<T> Failed()
    {
        return new TryGetResult<T> { IsSuccess = false };
    }
}