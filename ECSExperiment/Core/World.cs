using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Core
{
    public sealed class World
    {
        private int _nextId = 1;
        private readonly Stack<int> _free = new();
        private readonly Dictionary<Type, IComponentStore> _stores = new();
        private readonly HashSet<int> _alive = new();

        public Entity Create()
        {
            var id = _free.Count > 0 ? _free.Pop() : _nextId++;
            _alive.Add(id);
            return new Entity(id);
        }

        public void Destroy(Entity e)
        {
            if (!_alive.Remove(e.Id)) return;
            foreach (var store in _stores.Values) store.Remove(e);
            _free.Push(e.Id);
        }

        public bool Alive(Entity e) => _alive.Contains(e.Id);

        public ComponentStore<T> Store<T>()
        {
            if (!_stores.TryGetValue(typeof(T), out var store))
            {
                store = new ComponentStore<T>();
                _stores.Add(typeof(T), store);
            }
            return (ComponentStore<T>)store;
        }

        public void Add<T>(Entity e, in T component) => Store<T>().Set(e, component);
        public ref T Get<T>(Entity e) => ref Store<T>().GetRef(e);
        public bool TryGet<T>(Entity e, out T value) => Store<T>().TryGet(e, out value);
        public bool Has<T>(Entity e) => Store<T>().Has(e);
        public void Remove<T>(Entity e) => Store<T>().Remove(e);

        // Query: returns entities that have ALL listed component types.
        public IEnumerable<Entity> Query(params Type[] types)
        {
            if (types.Length == 0) yield break;

            // Start from the smallest store to minimize intersections.
            var stores = types.Select(t => (_stores.TryGetValue(t, out var s) ? s : null, t))
                              .Where(p => p.Item1 != null)
                              .OrderBy(p => p.Item1!.Count)
                              .ToArray();
            if (stores.Length != types.Length) yield break; // some component type has 0 entities

            foreach (var e in stores[0].Item1!.Entities())
            {
                bool ok = true;
                for (int i = 1; i < stores.Length; i++)
                {
                    if (!stores[i].Item1!.Has(e)) { ok = false; break; }
                }
                if (ok && Alive(e)) yield return e;
            }
        }

        // Event bus (simple, per-frame flushed)
        private readonly Dictionary<Type, List<object>> _events = new();

        public void Emit<TEvent>(in TEvent ev)
        {
            var t = typeof(TEvent);
            if (!_events.TryGetValue(t, out var list)) _events[t] = list = new List<object>();
            list.Add(ev!);
        }

        public IEnumerable<TEvent> Drain<TEvent>()
        {
            var t = typeof(TEvent);
            if (!_events.TryGetValue(t, out var list) || list.Count == 0) yield break;
            foreach (var obj in list) yield return (TEvent)obj;
            list.Clear();
        }
    }
}
