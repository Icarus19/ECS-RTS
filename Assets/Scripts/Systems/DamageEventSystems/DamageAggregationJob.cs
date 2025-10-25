using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Systems.DamageEventSystems
{
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    [UpdateAfter(typeof(ProjectileDamageSystem))]
    [UpdateAfter(typeof(MeleeDamageSystem))]
    [UpdateAfter(typeof(DamageOverTimeSystem))]
    public partial class DamageAggregationSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //throw new System.NotImplementedException();
        }
    }
    /*
    [BurstCompile]
    public partial struct DamageAggregationJob : IJob
    {
        public NativeQueue<NativeDamageEvent> Queue;
        public BufferLookup<DamageEvent> DamageBuffers;

        public void Execute()
        {
            while (Queue.TryDequeue(out var dmg))
            {
                if (DamageBuffers.HasBuffer(dmg.Target))
                {
                    var buffer = DamageBuffers[dmg.Target];
                    //buffer.Add(dmg);
                }
            }
        }
        
    }
*/
}