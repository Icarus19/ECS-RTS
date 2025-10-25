using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Helpers;
using Unity.Collections;

namespace Systems.DamageEventSystems
{
    [BurstCompile]
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    [UpdateAfter(typeof(ProjectileDamageSystem))]
    [UpdateAfter(typeof(MeleeDamageSystem))]
    [UpdateAfter(typeof(DamageOverTimeSystem))]
    public partial class DamageApplicationSystem : SystemBase
    {
        DamageEventManager _damageEventManager;

        protected override void OnCreate()
        {
            _damageEventManager = DamageEventManagerSingleton.Instance;
        }

        protected override void OnUpdate()
        {
            var events = _damageEventManager.Events;
            var job = new ApplyDamageJob
            {
                Events = events
            };

            job.ScheduleParallel();
            //state.Dependency.Complete(); // optional — ensures applied before clearing

            _damageEventManager.Reset();
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
}
/*
    [BurstCompile]
    public partial struct DamageEventSystem : ISystem
    {
        //private NativeQueue<CombatLogEntry> _combatLogQueue;

        public void OnCreate(ref SystemState state)
        {
            //_combatLogQueue = new NativeQueue<CombatLogEntry>(Allocator.Persistent);

            state.RequireForUpdate<DamageEvent>();
            //state.RequireForUpdate<CombatLog>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var job = new DamageEventJob();
            job.ScheduleParallel();
            var job = new DamageEventJob
            {
                CombatLogQueue = _combatLogQueue.AsParallelWriter(),
                Time = (float)SystemAPI.Time.ElapsedTime
            };
            job.ScheduleParallel();

            var logEntity = SystemAPI.GetSingletonEntity<CombatLog>();
            var buffer = state.EntityManager.GetBuffer<CombatLogEntry>(logEntity);

            while (_combatLogQueue.TryDequeue(out var entry))
            {
                buffer.Add(entry);
            }
        }

        public void OnDestroy(ref SystemState state)
        {
            //_combatLogQueue.Dispose();
        }
    }

    /// <summary>
    /// For future me.
    /// In order to have multiple entities create a damage event for the same entity in the same frame
    /// We first need to create NativeQueue<DamageEvent> and collect them in a damageAggregationJob
    /// </summary>
    [BurstCompile]
    public partial struct DamageEventJob : IJobEntity
    {
        [NativeDisableParallelForRestriction]
        public NativeQueue<CombatLogEntry>.ParallelWriter CombatLogQueue;
        public float Time;

        public void Execute(Entity entity,
            ref Health health,
            ref Shield shield,
            ref Armor armor,
            DynamicBuffer<DamageEvent> damageBuffer)
        {
            float totalDamage = 0f;

            for (int i = 0; i < damageBuffer.Length; i++)
            {
                var dmg = damageBuffer[i];
                float resistance = 0f;

                // Get the relevant resistance directly from the armor component
                switch (dmg.Type)
                {
                    case DamageType.Physical:
                        resistance = DamageCalculator.ArmorReduction(armor.PhysicalResistance);
                        break;
                    case DamageType.Magical:
                        resistance = DamageCalculator.ArmorReduction(armor.MagicalResistance);
                        break;
                    case DamageType.Fire:
                        resistance = DamageCalculator.ArmorReduction(armor.FireResistance);
                        break;
                    case DamageType.Ice:
                        resistance = DamageCalculator.ArmorReduction(armor.IceResistance);
                        break;
                    case DamageType.Poison:
                        resistance = DamageCalculator.ArmorReduction(armor.PoisonResistance);
                        break;
                }

                totalDamage += dmg.Value * resistance * armor.Amplification;
            }

            damageBuffer.Clear();

            float absorbed = math.min(shield.Value, totalDamage);
            shield.Value -= absorbed;
            health.Current -= (totalDamage - absorbed);
        }
    }
}*/