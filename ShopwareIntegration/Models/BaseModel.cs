using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ShopwareIntegration.Models
{
    public abstract class BaseModel
    {
        [JsonIgnore]
        public abstract string TableName { get; }
        ///<summary>
        /// Check if all Required attribtes are set
        ///</summary>
        internal void Validate()
        {
            bool allSet = AreAllRequiredValuesSet();
            if (allSet is false)
                throw new InvalidOperationException($"not all [Required] properties are set for {this.GetType()}");
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
