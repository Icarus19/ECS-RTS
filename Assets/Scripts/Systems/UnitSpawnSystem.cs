using System.Numerics;
using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct UnitSpawnSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SpawnRequest>();
        }
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(Allocator.Temp);

            var registry = SystemAPI.GetSingletonBuffer<UnitPrefabRegistry>();

            foreach (var (request, entity) in SystemAPI.Query<RefRO<SpawnRequest>>().WithEntityAccess())
            {
                for(var i = 0; i < registry.Length; i++)
                {
                    if (registry[i].Id == request.ValueRO.Id)
                    {
                        Entity instance = ecb.Instantiate(registry[i].Prefab);
                        ecb.SetComponent(instance, new LocalTransform
                        {
                            Position = request.ValueRO.Position,
                            Rotation = quaternion.identity,
                            Scale = 1f
                        });
                        break;
                    }
                }
                ecb.DestroyEntity(entity);
            }
            
            ecb.Playback(state.EntityManager);
            ecb.Dispose();
        }
    }
}