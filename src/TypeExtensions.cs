using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maquina
{
    public static class TypeExtensions
    {
        public static Dictionary<TKey, TValue> MergeWith<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionaryA,
            IDictionary<TKey, TValue> dictionaryB,
            bool useFirstOnDuplicate = false)
        {
            // Use first occurence when there is a duplicate
            if (useFirstOnDuplicate)
            {
                return dictionaryA.Concat(dictionaryB)
                    .GroupBy(e => e.Key)
                    .ToDictionary(g => g.Key, g => g.First().Value);
            }
            // Use the last (the one from dictionary B) when there is a duplicate
            return dictionaryA.Concat(dictionaryB)
                    .GroupBy(e => e.Key)
                    .ToDictionary(g => g.Key, g => g.Last().Value);
        }
    }
}
