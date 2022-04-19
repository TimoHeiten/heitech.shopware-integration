using System;
using System.Threading.Tasks;

namespace ShopwareIntegration.Requests
{
    ///<summary>
    /// Signals if the ShopwareClient Operation returned a successful Result
    ///</summary>
    internal class RequestResult<T>
    {
        private T Model { get; }
        private Failure? _failure { get; }
        private RequestResult(T model)
        {
            Model = model;
            _failure = null!;
        }

        private RequestResult(Failure failure)
        {
            Model = default!;
            _failure = failure;
        }

        internal static RequestResult<T> Success(T model) => new(model);
        internal static RequestResult<T> Failed(Exception ex, bool isNotAuthenticated) => new(new Failure(isNotAuthenticated, ex));

        internal async Task EvalAsync(Func<T, Task> onSuccess, Func<Failure, Task> onFailure)
        {
            if (IsSuccess)
            {
                await onSuccess(Model);
                return;
            }
            await onFailure(this._failure!);
        }

        internal bool IsSuccess => _failure is null;

        internal bool NotAuthenticated() => IsSuccess is false && _failure!.IsAuthenticationError;
    }
    public record Failure (bool IsAuthenticationError, Exception Exception);
}
