using Helpers;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems.DamageEventSystems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    [UpdateBefore(typeof(DamageApplicationSystem))]
    public partial struct DamageOverTimeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var writer = DamageEventManagerSingleton.Instance.AsParallelWriter();
            var lastHandle = DamageEventManagerSingleton.Instance.LastWriterHandle;
            var dt = SystemAPI.Time.DeltaTime;

            var dotJobHandle = new ApplyDoTJob
            {
                Writer = writer,
                DeltaTime = dt
            }.ScheduleParallel(lastHandle);

            DamageEventManagerSingleton.Instance.LastWriterHandle = dotJobHandle;
            state.Dependency = dotJobHandle;
        }
    }

    [BurstCompile]
    public partial struct ApplyDoTJob : IJobEntity
    {
        public NativeParallelMultiHashMap<Entity, NativeDamageEvent>.ParallelWriter Writer;
        public float DeltaTime;

        public void Execute(Entity entity, DynamicBuffer<DamageOverTime> dots)
        {
            for (int i = dots.Length - 1; i >= 0; i--)
            {
                var dot = dots[i];

                bool infinite = dot.Duration <= 0;
                
                if(!infinite)
                    dot.Duration -= DeltaTime;

                if (PeriodicEffectUtility.UpdateTick(ref dot.Timer, dot.Interval, DeltaTime))
                {
                    var evt = new NativeDamageEvent
                    {
                        Source = entity,
                        Target = entity,
                        Value = dot.Damage,
                        Type = dot.Type
                    };
                    
                    Writer.Add(entity, evt);

                    dot.Timer += dot.Interval;
                }
                
                if (!infinite && dot.Duration <= 0f)
                {
                    dots.RemoveAt(i);
                }
                else
                {
                    dots[i] = dot;
                }
            }
        }
    }
}