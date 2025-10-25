using Unity.Burst;
using Unity.Entities;

namespace Systems.DamageEventSystems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial class DamageEventPipelineSystemGroup : ComponentSystemGroup { }
}