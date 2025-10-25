using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct SizeModifierSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<AmplifySize>();
    }
    public void OnUpdate(ref SystemState state)
    {
        
    }
}
