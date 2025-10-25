using Unity.Entities;
using UnityEngine;

public class AbilityAuthoring : MonoBehaviour
{
    public string abilityName;
    public Targeting target;
    class Baker : Baker<AbilityAuthoring>
    {
        public override void Bake(AbilityAuthoring authoring)
        {
            Entity entity = GetEntity(authoring, TransformUsageFlags.None);
            AddComponent(entity, new AbilityTag
            {
                Target = authoring.target
            });
        }
    }
}