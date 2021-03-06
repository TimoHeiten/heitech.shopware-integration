using System;

namespace ShopwareIntegration.Requests
{
    ///<summary>
    /// Signals if the ShopwareClient Operation returned a successful Result
    ///</summary>
    public class RequestResult<T>
    {
        public T Model { get; }
        public Exception Exception { get; }
        private RequestResult(T model)
        {
            Model = model;
            Exception = null!;
        }

        private RequestResult(Exception ex)
        {
            Model = default!;
            Exception = ex;
        }

        internal static RequestResult<T> Success(T model) => new(model);
        internal static RequestResult<T> Failed(Exception ex) => new(ex);

        public bool IsSuccess => Exception is null;
    }
}
