using System;

namespace ShopwareIntegration.Requests
{
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

        public static RequestResult<T> Success(T model) => new(model);
        public static RequestResult<T> Failed(Exception ex) => new(ex);

        public bool IsSuccess => Exception is null;
    }
}
