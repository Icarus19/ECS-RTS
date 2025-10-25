using Unity.Entities;

namespace Systems.DamageEventSystems
{
    
    [UpdateInGroup(typeof(DamageEventPipelineSystemGroup))]
    public partial class ProjectileDamageSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            //throw new System.NotImplementedException();
        }
    }
}