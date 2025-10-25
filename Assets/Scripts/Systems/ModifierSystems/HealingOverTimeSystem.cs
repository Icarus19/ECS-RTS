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

            foreach (var (health, hots) in
                     SystemAPI.Query<RefRW<Health>, DynamicBuffer<HealingOverTime>>())
            {
                // Iterate backwards for safe removal
                for (int i = hots.Length - 1; i >= 0; i--)
                {
                    // Get a ref to the element instead of a copy
                    ref var hot = ref hots.ElementAt(i);

                    bool infinite = hot.Duration <= 0;
                    
                    // Reduce duration
                    if (!infinite)
                        hot.Duration -= dt;

                    // Perform tick check using your shared utility
                    if (PeriodicEffectUtility.UpdateTick(ref hot.Timer, hot.Interval, dt))
                    {
                        health.ValueRW.Current = math.min(
                            health.ValueRO.Max,
                            health.ValueRO.Current + hot.Heal);
                    }

                    // Remove if expired
                    if (!infinite && hot.Duration <= 0f)
                    {
                        hots.RemoveAt(i);
                    }
                }
            }
        }
    }
}