using System;

namespace models
{
    public abstract class BaseEntity<T> : IEquatable<T>
    {
        public abstract bool Equals(T? otherId);
        
    }
}
