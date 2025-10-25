using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Systems.DamageEventSystems
{
    
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    public partial struct MeleeDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            /*var writer = DamageEventManagerSingleton.Instance.Stream.AsWriter();

            // Schedule a job to enqueue damage events
            new MeleeDamageJob
            {
                Stream = writer
            }.ScheduleParallel();
        }

        [BurstCompile]
        public partial struct MeleeDamageJob : IJobEntity
        {
            public NativeStream.Writer Stream;

            public void Execute(Entity entity, in Target target, in MeleeAttack attack)
            {
                
            }*/
        }
    }
}