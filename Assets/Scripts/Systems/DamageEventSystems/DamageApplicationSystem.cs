using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Helpers;
using Unity.Collections;
using Unity.Jobs;

namespace Systems.DamageEventSystems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    //[UpdateAfter(typeof(ProjectileDamageSystem))]
    //[UpdateAfter(typeof(MeleeDamageSystem))]
    [UpdateAfter(typeof(DamageOverTimeSystem))]
    public partial struct DamageApplicationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var events = DamageEventManagerSingleton.Instance.Events;
            var lastHandle = DamageEventManagerSingleton.Instance.LastWriterHandle;
            
            var applyJobHandle = new ApplyDamageJob
            {
                Events = events
            }.ScheduleParallel(lastHandle);

            state.Dependency = applyJobHandle;
            
            var clearJobHandle = new ClearDamageEventsJob
            {
                Events = events
            }.Schedule(applyJobHandle);
            
            state.Dependency = clearJobHandle;
            DamageEventManagerSingleton.Instance.LastWriterHandle = clearJobHandle;
        }
    }

    [BurstCompile]
    public partial struct ApplyDamageJob : IJobEntity
    {
        [ReadOnly] public NativeParallelMultiHashMap<Entity, NativeDamageEvent> Events;

        public void Execute(
            Entity entity,
            ref Health health,
            ref Shield shield,
            in Armor armor,
            in DynamicBuffer<ArmorModifier> modifiers)
        {
            float totalDamage = 0f;

            // Aggregate armor modifiers
            float armorBonusPhysical = 0f,
                armorBonusMagical = 0f,
                armorBonusHoly = 0f,
                armorBonusFire = 0f,
                armorBonusIce = 0f,
                armorBonusPoison = 0f;
            float ampBonus = 1f;

            for (int i = 0; i < modifiers.Length; i++)
            {
                armorBonusPhysical += modifiers[i].PhysicalResistance;
                armorBonusMagical += modifiers[i].MagicalResistance;
                armorBonusHoly += modifiers[i].HolyResistance;
                armorBonusFire += modifiers[i].FireResistance;
                armorBonusIce += modifiers[i].IceResistance;
                armorBonusPoison += modifiers[i].PoisonResistance;
                ampBonus *= modifiers[i].Amplification;
            }

            if (!Events.TryGetFirstValue(entity, out var evt, out var it))
                return;

            do
            {
                float resistance = evt.Type switch
                {
                    DamageType.Physical => DamageCalculator.ArmorReduction(
                        armor.PhysicalResistance + armorBonusPhysical),
                    DamageType.Magical => DamageCalculator.ArmorReduction(
                        armor.MagicalResistance + armorBonusMagical),
                    DamageType.Holy => DamageCalculator.ArmorReduction(
                        armor.HolyResistance + armorBonusHoly),
                    DamageType.Fire => DamageCalculator.ArmorReduction(
                        armor.FireResistance + armorBonusFire),
                    DamageType.Ice => DamageCalculator.ArmorReduction(
                        armor.IceResistance + armorBonusIce),
                    DamageType.Poison => DamageCalculator.ArmorReduction(
                        armor.PoisonResistance + armorBonusPoison),
                    _ => 1f
                };

                totalDamage += evt.Value * resistance * armor.Amplification * ampBonus;

            } while (Events.TryGetNextValue(out evt, ref it));

            float absorbed = math.min(shield.Value, totalDamage);
            shield.Value -= absorbed;
            health.Current -= (totalDamage - absorbed);
        }
    }
    [BurstCompile]
    public struct ClearDamageEventsJob : IJob
    {
        public NativeParallelMultiHashMap<Entity, NativeDamageEvent> Events;

        public void Execute()
        {
            if (Events.IsCreated)
                Events.Clear();
        }
    }
}