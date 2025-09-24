using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Core
{
    public sealed class ComponentStore<T> : IComponentStore
    {
        private readonly Dictionary<int, T> _data = new();
        public bool Has(Entity e) => _data.ContainsKey(e.Id);
        //public ref T GetRef(Entity e) => ref CollectionsMarshal.GetValueRefOrAddDefault(_data, e.Id, out _);
        public ref T GetRef(Entity e) => ref _data.GetValueRefOrAddDefault(e.Id, out _);
        public bool TryGet(Entity e, out T value) => _data.TryGetValue(e.Id, out value);
        public void Set(Entity e, in T value) => _data[e.Id] = value;
        public void Remove(Entity e) => _data.Remove(e.Id);
        public IEnumerable<Entity> Entities() { foreach (var k in _data.Keys) yield return new Entity(k); }
        public int Count => _data.Count;
    }
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
