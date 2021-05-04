using System.Collections.Generic;

namespace GetAwsMetric
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> map, TKey key) => map.TryGetValue(key, out var val) ? val : default;
    }
}
