using System.Collections.Generic;

namespace Rhendaria.Abstraction.Extensions
{
    public static class HashSetExtensions
    {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> collection)
        {
            return new HashSet<T>(collection);
        }

        public static HashSet<T> ExceptOf<T>(this HashSet<T> set, T some)
        {
            var clone = set.ToSet();
            clone.ExceptWith(new[] { some });
            return clone;
        }
    }
}