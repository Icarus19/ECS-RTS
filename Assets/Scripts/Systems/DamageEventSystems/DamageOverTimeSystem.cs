using Helpers;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems.DamageEventSystems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    [UpdateBefore(typeof(DamageApplicationSystem))]
    public partial class DamageOverTimeSystem : SystemBase
    {
        DamageEventManager _damageEventManager;

        protected override void OnCreate()
        {
            _damageEventManager = DamageEventManagerSingleton.Instance;
        }
        protected override void OnUpdate()
        {
            var writer = _damageEventManager.AsParallelWriter();
            var dt = SystemAPI.Time.DeltaTime;
            
            new ApplyDoTJob
            {
                Writer = writer,
                DeltaTime = dt
            }.ScheduleParallel();
        }
        [BurstCompile]
        public partial struct ApplyDoTJob : IJobEntity
        {
            public NativeParallelMultiHashMap<Entity, NativeDamageEvent>.ParallelWriter Writer;
            public float DeltaTime;

            public void Execute(Entity entity, DynamicBuffer<DamageOverTime> dots)
            {
                for (int i = 0; i < dots.Length; i++)
                {
                    var dot = dots[i];
                    
                    if(dot.Duration >0)
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
                    
                    if (dot.Duration <= 0f)
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
}