using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public partial struct ArmorModifierSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Armor>();
        state.RequireForUpdate<ArmorModifier>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach(var (armor, buffer) in SystemAPI.Query<RefRW<Armor>, DynamicBuffer<ArmorModifier>>())
        {
            for (int i = 0; i < buffer.Length; ++i)
            {
                if (buffer[i].Duration > 0)
                {
                    ref var mod = ref buffer.ElementAt(i);
                    
                    mod.Duration -= SystemAPI.Time.DeltaTime;
                    
                    if(mod.Duration <= 0)
                        buffer.RemoveAt(i);
                }
            }
        }
    }
}