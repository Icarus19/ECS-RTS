using Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    public partial struct LifetimeSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Lifetime>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            foreach (var (lifetime, entity) in
                     SystemAPI.Query<RefRW<Lifetime>>().WithEntityAccess())
            {
                lifetime.ValueRW.Value -= SystemAPI.Time.DeltaTime;

                if (lifetime.ValueRW.Value <= 0f)
                {
                    ecb.DestroyEntity(entity);
                }
            }

            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}