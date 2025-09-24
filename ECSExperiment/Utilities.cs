using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment
{
    file static class DictionaryExtensions
    {
        // .NET 8 has CollectionsMarshal.GetValueRefOrAddDefault; this is a safe stand-in
        // for clarity. It’s not as fast, but fine for a console game prototype.
        public static ref TValue GetValueRefOrAddDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, out bool existed)
            where TKey : notnull
        {
            if (dict.TryGetValue(key, out var value))
            {
                existed = true;
                return ref Unsafe.AsRef(value);
            }
            existed = false;
            dict[key] = default!;
            //return ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out existed);
            return ref dict[key];
        }
    }
}
