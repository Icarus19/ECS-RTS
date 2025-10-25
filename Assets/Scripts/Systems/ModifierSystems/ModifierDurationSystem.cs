using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct ModifierDurationSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        float dt = SystemAPI.Time.DeltaTime;
        var ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (duration, entity) in SystemAPI.Query<RefRW<Modifier>>().WithEntityAccess())
        {
            if (duration.ValueRO.Duration < 0)
                continue;
        
            duration.ValueRW.Duration -= dt;
            
            if (duration.ValueRW.Duration <= 0)
                ecb.DestroyEntity(entity);
        }

        ecb.Playback(state.EntityManager);
    }
}

/*
[BurstCompile]
public partial struct ModifierDurationSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        new ModifierDurationJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel();
    }

    public void OnDestroy(ref SystemState state)
    {
    }
}[BurstCompile]
public partial struct ModifierDurationJob : IJobEntity
{
    public float DeltaTime;

    public void Execute(DynamicBuffer<Modifier> modifiers)
    {
        for (int i = 0; i < modifiers.Length; i++)
        {
            var m =  modifiers[i];
            if (m.Duration > 0f)
            {
                m.Duration -= DeltaTime;

                if (m.Duration <= 0f)
                    modifiers.RemoveAt(i);
                else
                    modifiers[i] = m;
            }
        }
    }
}
*/