using Unity.Burst;
using Unity.Entities;

[BurstCompile]
[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct AbilitySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<CastAbilityRequest>();
        state.RequireForUpdate<PrimarySelection>();
    }

    public void OnUpdate(ref SystemState state)
    {
        
    }

    public void OnDestroy(ref SystemState state)
    {
    }
}