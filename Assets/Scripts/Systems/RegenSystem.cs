using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct RegenSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Regenable>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;

        foreach (var health in SystemAPI.Query<RefRW<Health>>().WithAll<Regenable>())
        {
            ApplyRegen(ref health.ValueRW.Current, health.ValueRO.Max, health.ValueRO.Regen, dt);
        }

        foreach (var mana in SystemAPI.Query<RefRW<Mana>>().WithAll<Regenable>())
        {
            ApplyRegen(ref mana.ValueRW.Current, mana.ValueRO.Max, mana.ValueRO.Regen, dt);
        }
    }

    static void ApplyRegen(ref float current, float max, float regen, float deltaTime)
    {
        current = math.min(max, current + regen * deltaTime);
    }

    public void OnDestroy(ref SystemState state)
    {
    }
}
/* This might become necessary to fix and implement down the line. Just change Execute from using floats, to directly use components like health.Current
[BurstCompile]
public partial struct RegenSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Regenable>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var dt = SystemAPI.Time.DeltaTime;
        
        var ApplyRegenJob =  new ApplyRegenJob
        {
            deltaTime = dt,
        };
        state.Dependency = ApplyRegenJob.ScheduleParallel(state.Dependency);
    }

    public void OnDestroy(ref SystemState state)
    {
    }
}

[BurstCompile]
[WithAll(typeof(Regenable))]
public partial struct ApplyRegenJob : IJobEntity
{
    public float deltaTime;
    void Execute(ref float current, float max, float regen)
    {
        current = math.min(max, current + regen * deltaTime);
    }
}*/