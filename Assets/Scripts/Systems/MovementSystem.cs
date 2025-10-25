using Unity.Burst;
using Unity.Entities;
using UnityEngine.InputSystem;

[BurstCompile]
public partial struct MovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Movement>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var spatialSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpatialHashSystem>();
        var hashMap = spatialSystem.HashMap;
    }

    public void OnDestroy(ref SystemState state)
    {
    }
}