using Unity.Collections;
using Unity.Entities;

namespace Systems.DamageEventSystems
{
    public static class DamageEventManagerSingleton
    {
        static DamageEventManager _instance;
        public static DamageEventManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DamageEventManager(1024);
                return _instance;
            }
        }

        public static void DisposeInstance()
        {
            _instance?.Dispose();
            _instance = null;
        }
    }
    public class DamageEventManager
    {
        public NativeParallelMultiHashMap<Entity, NativeDamageEvent> Events;

        public DamageEventManager(int capacity)
        {
            Events = new NativeParallelMultiHashMap<Entity, NativeDamageEvent>(capacity, Allocator.Persistent);
        }

        public void Dispose()
        {
            if (Events.IsCreated)
                Events.Dispose();
        }

        public void Reset()
        {
            if (Events.IsCreated)
                Events.Clear();
        }

        public void AddEvent(NativeDamageEvent dmg)
        {
            Events.Add(dmg.Target, dmg);
        }
        public NativeParallelMultiHashMap<Entity, NativeDamageEvent>.ParallelWriter AsParallelWriter()
        {
            return Events.AsParallelWriter();
        }
    }
    public struct NativeDamageEvent
    {
        public Entity Target;
        public Entity Source;
        public float Value;
        public DamageType Type;
    }
}