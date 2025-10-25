using Unity.Entities;

namespace Systems.DamageEventSystems
{
    
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    public partial struct ProjectileDamageSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            //throw new System.NotImplementedException();
        }
    }
}