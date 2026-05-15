using Kamirion.RepairShop.Shared.Results;

namespace Kamirion.RepairShop.Shared.Behaviors;

internal static class ResultFactory
{
    // Result<T> constructor is protected internal, so we use reflection to create failures
    // from generic pipeline behaviors where TResponse : Result.
    internal static TResult Failure<TResult>(Error error) where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (TResult)(object)Result.Failure(error);

        var genericArg = typeof(TResult).GetGenericArguments()[0];
        return (TResult)typeof(Result)
            .GetMethod(nameof(Result.Failure), [typeof(Error)])!
            .MakeGenericMethod(genericArg)
            .Invoke(null, [error])!;
    }
}
