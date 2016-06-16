using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConfigurationManager.WebService.Helpers
{
    public static class KeyValuePairExtensions
    {
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            var pairsArr = pairs.ToArray();
            var dictionary = new Dictionary<TKey, TValue>(pairsArr.Length);

            foreach (var pair in pairs)
            {
                dictionary.Add(pair.Key, pair.Value);
            }

            return dictionary;
        }
    }
}