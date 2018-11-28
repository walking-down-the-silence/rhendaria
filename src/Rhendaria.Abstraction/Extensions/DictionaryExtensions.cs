using System.Collections.Generic;
using System.Linq;

namespace Rhendaria.Abstraction.Extensions
{
    public static class DictionaryExtensions
    {
        public static List<T> ExceptOf<T>(this Dictionary<string, T> dictionary, string some)
        {
            var items = dictionary
                .Where(k => k.Key != some)
                .Select(kvp => kvp.Value)
                .ToList();

            return items;
        }
    }
}