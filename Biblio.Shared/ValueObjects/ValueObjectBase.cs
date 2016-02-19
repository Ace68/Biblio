using System.Collections.Generic;
using System.Linq;

namespace Biblio.Shared.ValueObjects
{
    public abstract class ValueObjectBase<T> where T : ValueObjectBase<T>
    {
        protected abstract IEnumerable<object> GetAttributesToIncludeInEqualityCheck();

        public override bool Equals(object other)
        {
            return this.Equals(other as T);
        }

        public bool Equals(T other)
        {
            if (other == null) return false;

            return
                this.GetAttributesToIncludeInEqualityCheck()
                    .SequenceEqual(other.GetAttributesToIncludeInEqualityCheck());
        }

        public static bool operator ==(ValueObjectBase<T> left, ValueObjectBase<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObjectBase<T> left, ValueObjectBase<T> right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return this.GetAttributesToIncludeInEqualityCheck()
                .Aggregate(17, (current, obj) => current * 31 + (obj?.GetHashCode() ?? 0));
        }
    }
}
