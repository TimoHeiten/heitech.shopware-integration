using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;

namespace ShopwareIntegration.Models
{
    public abstract class BaseModel
    {
        public abstract string TableName { get; }
        public HttpContent ToHttpContent()
        {
            bool allSet = AreAllRequiredValuesSet();
            if (allSet is false)
            {
                throw new InvalidOperationException($"not all [Required] properties are set for {this.GetType()}");
            }
            return JsonContent.Create(this);
        }

        protected virtual bool AreAllRequiredValuesSet()
        {
            // checks if all the Required Properties are set to non default values
            // can be overwritten for any given baseModel subType
            var requiredProperties = this.GetType().GetProperties().Where(x => x.GetCustomAttribute<RequiredAttribute>() is not null);
            return requiredProperties.All(x => !object.Equals(x, default));
        }

    }
}
