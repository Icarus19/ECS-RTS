using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct DamageTestBootstrapSystem : ISystem
{
    Entity testEntity;
    Entity logEntity;

    public void OnCreate(ref SystemState state)
    {
        var em = state.EntityManager;
        logEntity = em.CreateEntity(typeof(CombatLog));
        em.AddBuffer<CombatLogEntry>(logEntity);
        
        testEntity = em.CreateEntity(typeof(Health), typeof(Shield), typeof(Armor), typeof(ArmorModifier), typeof(HealingOverTime), typeof(DamageOverTime));
        em.SetComponentData(testEntity, new Health { Current = 100, Max = 200 });
        em.SetComponentData(testEntity, new Shield { Value = 80 });
        em.SetComponentData(testEntity, new Armor { Amplification = 1f, PhysicalResistance = 50 });
        
        

        Debug.Log("Created test entity");
    }

    public void OnUpdate(ref SystemState state)
    {
        var armor = state.EntityManager.GetBuffer<ArmorModifier>(testEntity);
        var hot = state.EntityManager.GetBuffer<HealingOverTime>(testEntity);
        var dot = state.EntityManager.GetBuffer<DamageOverTime>(testEntity);

        armor.Add(new ArmorModifier { Amplification = 1f, FireResistance = 20f, Duration = 30f });
        hot.Add(new HealingOverTime { Duration = 20f, Heal = 50f, Interval = 10f });
        dot.Add(new DamageOverTime { Damage = 20f, Interval = 1f, Type = DamageType.Fire, Duration = -1f });
        dot.Add(new DamageOverTime { Damage = 2f, Interval = 0.2f, Type = DamageType.Holy, Duration = 30f });
        //armor.Add(new ArmorModifier { Amplification = 1f, PhysicalResistance = 50 });
        // Add three damage hits once
        /*
        var buffer = state.EntityManager.GetBuffer<NativeDamageEvent>(testEntity);
        buffer.Add(new DamageEvent { Value = 10, Type = DamageType.Fire });
        buffer.Add(new DamageEvent { Value = 25, Type = DamageType.Ice });
        buffer.Add(new DamageEvent { Value = 40, Type = DamageType.Physical });
        */
        

        Debug.Log("Added 3 damage events (10, 25, 40)");

        // Disable this system so it doesn't repeat every frame
        state.Enabled = false;
    }
}