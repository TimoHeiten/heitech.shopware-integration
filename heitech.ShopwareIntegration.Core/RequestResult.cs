using System;
using heitech.ShopwareIntegration.Core.Data;

namespace heitech.ShopwareIntegration.Core
{
    ///<summary>
    /// Encapsulates a Result from the Shopware-Api and holds information about the success of the Result
    ///</summary>
    public sealed class RequestResult<T>
        where T : ShopwareDataContainer
    {
        public T Model { get; }
        public Exception Exception { get; }

        private RequestResult(T model)
        {
            Model = model;
            Exception = null;
        }

        private RequestResult(Exception ex)
        {
            Model = default;
            Exception = ex;
        }

        ///<summary>
        /// Creates a Successful RequestResult
        ///</summary>
        internal static RequestResult<T> Success(T model) => new RequestResult<T>(model);
        ///<summary>
        /// Creates a failed RequestResult
        ///</summary>
        internal static RequestResult<T> Failed(Exception ex) => new RequestResult<T>(ex);
        public bool IsSuccess => Exception is null;
    }
}