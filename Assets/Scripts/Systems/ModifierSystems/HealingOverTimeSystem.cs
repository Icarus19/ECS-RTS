using Helpers;
using Systems.DamageEventSystems;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems.ModifierSystems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    [UpdateBefore(typeof(DamageApplicationSystem))]
    public partial struct HealingOverTimeSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float dt = SystemAPI.Time.DeltaTime;

            new HealingTickJob
            {
                DeltaTime = dt
            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct HealingTickJob : IJobEntity
        {
            public float DeltaTime;

            public void Execute(Entity entity, RefRW<Health> health, DynamicBuffer<HealingOverTime> hots)
            {
                for (int i = hots.Length - 1; i >= 0; i--)
                {
                    var hot = hots[i];

                    if(hot.Duration >= 0)
                        hot.Duration -= DeltaTime;

                    // Tick check using shared helper
                    if (PeriodicEffectUtility.UpdateTick(ref hot.Timer, hot.Interval, DeltaTime))
                    {
                        health.ValueRW.Current = math.min(
                            health.ValueRO.Max,
                            health.ValueRO.Current + hot.Heal);
                    }

                    if (hot.Duration <= 0f)
                    {
                        hots.RemoveAt(i);
                    }
                    else
                    {
                        hots[i] = hot;
                    }
                }
            }
        }
    }

}